using UnityEngine;

public class BlockScript : MonoBehaviour
{
    private Vector3 offset; // Смещение между мышью и блоком
    private bool isDragging = false; // Флаг, отслеживающий, перетаскивается ли блок

    // Ссылки на блоки, присоединенные сверху и снизу
    private BlockScript connectedBlockAbove = null;
    private BlockScript connectedBlockBelow = null;

    private void OnMouseDown()
    {
        // Запоминаем смещение при нажатии мыши
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            // Перемещаем блок вместе с мышью
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        TryConnect();
    }

    // Получение позиции мыши в мировых координатах
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z; // Учитываем позицию камеры
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void TryConnect()
    {
        float connectionDistance = 1.0f; // Максимальное расстояние для соединения блоков

        // Ищем все блоки в сцене
        BlockScript[] allBlocks = FindObjectsOfType<BlockScript>();
        foreach (BlockScript otherBlock in allBlocks)
        {
            if (otherBlock == this) continue; // Пропускаем сам себя

            // Проверяем возможность соединения сверху
            float distanceAbove = Vector3.Distance(transform.position, otherBlock.transform.position + Vector3.up * connectionDistance);
            if (distanceAbove < connectionDistance && otherBlock.connectedBlockBelow == null && connectedBlockAbove == null)
            {
                ConnectBlockAbove(otherBlock);
                return;
            }

            // Проверяем возможность соединения снизу
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
        // Соединяем текущий блок снизу к другому блоку
        connectedBlockAbove = otherBlock;
        otherBlock.connectedBlockBelow = this;
        transform.position = otherBlock.transform.position + Vector3.down; // Подстраиваем позицию блока
    }

    private void ConnectBlockBelow(BlockScript otherBlock)
    {
        // Соединяем текущий блок сверху к другому блоку
        connectedBlockBelow = otherBlock;
        otherBlock.connectedBlockAbove = this;
        transform.position = otherBlock.transform.position + Vector3.up; // Подстраиваем позицию блока
    }
}
