using UnityEngine;
using System.Collections;

public class Vehicle_Controller : MonoBehaviour
{
    public Animator animator;
    public bool isAtRoom1 = true;      // true: Room1, false: Room2 앞
    public bool isMoving = false;      // 이동 중 여부
    public bool playerOnBoard = false; // 플레이어 탑승 여부

    private bool isInterrupted = false;
    private float pausedAnimTime = 0f;
    private string currentMovementAnim = "";
    private Transform player;
    private float currentProgress = 0f;

    public bool hasArrived = false;
    public float arrivalCooldown = 1.0f;
    public float arrivalTimestamp = -1000f;

    public bool justArrived = false;

    private void Update()
    {
        if (isMoving && !isInterrupted)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            currentProgress = state.normalizedTime % 1f;

            if (state.IsName(currentMovementAnim) && currentProgress >= 0.99f)
            {
                OnArrival(); // 정상 도착 처리
            }
        }
    }

    public void BoardVehicle(Transform playerTransform)
    {
        if (hasArrived)
        {
            hasArrived = false;
        }
        if (isMoving && !isInterrupted) return;

        playerOnBoard = true;
        player = playerTransform;

        playerTransform.SetParent(transform);
        playerTransform.position = transform.position + Vector3.up * 2f;

        if (isInterrupted)
        {
            animator.speed = 1;
            animator.Play(currentMovementAnim, 0, pausedAnimTime);
            isMoving = true;
            isInterrupted = false;
            Debug.Log("재탑승: 중단된 애니메이션 재개, 진행률: " + pausedAnimTime);
        }
        else
        {
            currentMovementAnim = isAtRoom1 ? "Moveto_Room2" : "Moveto_Room1";
            animator.Play(currentMovementAnim);
            isMoving = true;
        }
    }

    public void ExitVehicle(bool forceExit = false)
    {
        if (!isMoving && !forceExit) return;

        // 도착 직후 강제 하차 방지
        if (hasArrived && justArrived)
        {
            Debug.Log("도착 후 잠시 동안 하차 무시");
            return;
        }

        if (!playerOnBoard) return;

        if (isMoving && forceExit)
        {
            pausedAnimTime = currentProgress;
            animator.speed = 0;
            isInterrupted = true;
            isMoving = false;
            Debug.Log("강제 하차: 저장된 진행률(" + pausedAnimTime + ")로 정지");
        }

        playerOnBoard = false;

        if (player != null)
        {
            player.SetParent(null);

            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = true;
            }

            player = null;
        }
    }

    // 🔄 기존: OnArrival은 private이었음 → public으로 변경
    public void OnArrival()
    {
        Debug.Log("Vehicle 도착. 현재 위치: " + (isAtRoom1 ? "Room2" : "Room1"));

        hasArrived = true;
        isMoving = false;

        // 위치 반영
        isAtRoom1 = !isAtRoom1;

        if (playerOnBoard)
        {
            ExitVehicle(false);
        }

        // 정지 애니메이션으로 전환
        if (isAtRoom1)
            animator.Play("Vehicle_Idle");
        else
            animator.Play("Vehicle_Room2");

        // 도착 쿨다운 처리
        arrivalTimestamp = Time.time;
        justArrived = true;
        StartCoroutine(ClearJustArrived(arrivalCooldown));
    }


    private IEnumerator ClearJustArrived(float delay)
    {
        yield return new WaitForSeconds(delay);
        justArrived = false;
        Debug.Log("justArrived 해제됨");
    }
}
