using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 문 정면 방향
        Vector3 doorForward = transform.forward;
        // 문 기준, 플레이어 위치 벡터
        Vector3 toPlayer = (other.transform.position - transform.position).normalized;

        // 플레이어가 문 정면에서 왔는지 후면에서 왔는지 판단
        float dot = Vector3.Dot(doorForward, toPlayer);

        // 뒤에서 접근하면 역방향 열기
        bool reverse = dot < 0;

        animator.SetBool("Reverse", reverse);
        animator.SetInteger("status", 1); // 열기 (Opening or Opening_Reverse)
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        animator.SetInteger("status", 2); // 닫기 (Closing or Closing_Reverse)
    }
}
