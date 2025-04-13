using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // �� ���� ����
        Vector3 doorForward = transform.forward;
        // �� ����, �÷��̾� ��ġ ����
        Vector3 toPlayer = (other.transform.position - transform.position).normalized;

        // �÷��̾ �� ���鿡�� �Դ��� �ĸ鿡�� �Դ��� �Ǵ�
        float dot = Vector3.Dot(doorForward, toPlayer);

        // �ڿ��� �����ϸ� ������ ����
        bool reverse = dot < 0;

        animator.SetBool("Reverse", reverse);
        animator.SetInteger("status", 1); // ���� (Opening or Opening_Reverse)
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        animator.SetInteger("status", 2); // �ݱ� (Closing or Closing_Reverse)
    }
}
