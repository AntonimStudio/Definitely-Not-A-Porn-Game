using UnityEngine;

public class BlockScript : MonoBehaviour
{
    private Vector3 offset; // �������� ����� ����� � ������
    private bool isDragging = false; // ����, �������������, ��������������� �� ����

    // ������ �� �����, �������������� ������ � �����
    private BlockScript connectedBlockAbove = null;
    private BlockScript connectedBlockBelow = null;

    private void OnMouseDown()
    {
        // ���������� �������� ��� ������� ����
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            // ���������� ���� ������ � �����
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        TryConnect();
    }

    // ��������� ������� ���� � ������� �����������
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z; // ��������� ������� ������
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void TryConnect()
    {
        float connectionDistance = 1.0f; // ������������ ���������� ��� ���������� ������

        // ���� ��� ����� � �����
        BlockScript[] allBlocks = FindObjectsOfType<BlockScript>();
        foreach (BlockScript otherBlock in allBlocks)
        {
            if (otherBlock == this) continue; // ���������� ��� ����

            // ��������� ����������� ���������� ������
            float distanceAbove = Vector3.Distance(transform.position, otherBlock.transform.position + Vector3.up * connectionDistance);
            if (distanceAbove < connectionDistance && otherBlock.connectedBlockBelow == null && connectedBlockAbove == null)
            {
                ConnectBlockAbove(otherBlock);
                return;
            }

            // ��������� ����������� ���������� �����
            float distanceBelow = Vector3.Distance(transform.position, otherBlock.transform.position + Vector3.down * connectionDistance);
            if (distanceBelow < connectionDistance && otherBlock.connectedBlockAbove == null && connectedBlockBelow == null)
            {
                ConnectBlockBelow(otherBlock);
                return;
            }
        }
    }

    private void ConnectBlockAbove(BlockScript otherBlock)
    {
        // ��������� ������� ���� ����� � ������� �����
        connectedBlockAbove = otherBlock;
        otherBlock.connectedBlockBelow = this;
        transform.position = otherBlock.transform.position + Vector3.down; // ������������ ������� �����
    }

    private void ConnectBlockBelow(BlockScript otherBlock)
    {
        // ��������� ������� ���� ������ � ������� �����
        connectedBlockBelow = otherBlock;
        otherBlock.connectedBlockAbove = this;
        transform.position = otherBlock.transform.position + Vector3.up; // ������������ ������� �����
    }
}
