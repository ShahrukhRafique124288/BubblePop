using System.Collections.Generic;
using UnityEngine;

public class AimController : GamePlayStateBaseController
{
    #region Variables & Properties

    private AimViewController m_aimViewController;
    private ShootModel m_shootModel;
    
    private Vector3 m_mousePosition;
    private Vector3 m_raycastStartPosition;
    private Vector3 m_bubblePosition;
    
    private Vector2 m_rayCastDirection;
    private Vector2 m_bubbleThrowPosition;
    
    private bool m_isLastLayerLeft;
    private bool m_isLastLayerRight;
    private bool m_canShootBubble;

    private int m_reflectionCount;
    private int m_gridPosition;
    private readonly int m_maxReflectionCount = 3;
    
    private LayerMask m_rightLayerMask;
    private LayerMask m_leftLayerMask;
    private LayerMask m_defaultLayerMask;
    
    private RaycastHit2D m_raycastHit;

    private Camera m_facingCamera;

    #endregion

    #region Initialization

    public AimController()
    {
        Initialize();
    }

    private void Initialize()
    {
        m_aimViewController = new AimViewController();

        m_shootModel = new ShootModel();
        m_shootModel.m_bubbleMovingPositions = new List<Vector3>();

        m_facingCamera = Camera.main;

        m_defaultLayerMask = ~(1 << 11);
        m_rightLayerMask = ~(1 << 10);
        m_leftLayerMask = ~(1 << 8);
    }

    #endregion Initialization

    #region Game Flow

    public void StartGame()
    {
     
    }
    
    public void PauseGame()
    {
        Hide();
        m_canShootBubble = false;
    }
    
    public void ResumeGame()
    {
     
    }

    #endregion Game Flow

    #region Aiming

    public void AimToShoot()
    {
        if (Input.GetMouseButton(0))
        {
            m_mousePosition = Input.mousePosition;

            m_reflectionCount = 1;
            m_isLastLayerLeft = false;
            m_isLastLayerRight = false;
            m_shootModel.m_bubbleMovingPositions.Clear();

            m_aimViewController.SetLineRendererPositionCount(m_reflectionCount);
            
            m_raycastStartPosition = m_aimViewController.m_RaycastStartPosition;

            m_rayCastDirection = m_facingCamera.ScreenToWorldPoint(m_mousePosition) - m_raycastStartPosition;

            m_canShootBubble = RayCastToAim(m_raycastStartPosition, m_rayCastDirection);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            Hide();

            if (m_canShootBubble)
            {
                EventManager.DoFireShootBubbleEvent(m_shootModel);
            }
        }
    }

    private bool RayCastToAim(Vector2 startPosition, Vector2 direction)
    {
        LayerMask m_layerToIgnore = m_isLastLayerLeft ? m_leftLayerMask : m_isLastLayerRight ? m_rightLayerMask : m_defaultLayerMask;

        m_raycastHit = Physics2D.Raycast(startPosition, direction, 10, m_layerToIgnore);

        if (m_raycastHit && m_reflectionCount < m_maxReflectionCount)
        {
            m_reflectionCount++;

            m_bubblePosition = m_facingCamera.WorldToScreenPoint(m_raycastHit.point);
            m_bubblePosition = m_facingCamera.ScreenToWorldPoint(m_bubblePosition);

            m_bubblePosition = m_aimViewController.GetBubbleLocalPosition(m_bubblePosition);

            m_aimViewController.SetLineRendererPosition(m_reflectionCount, m_reflectionCount - 1, m_bubblePosition);

            bool isCurrentLayerLeft = m_raycastHit.collider.gameObject.CompareTag(GameConstants.g_leftWallTag);
            bool isCurrentLayerRight = m_raycastHit.collider.gameObject.CompareTag(GameConstants.g_rightWallTag);

            if (isCurrentLayerLeft || isCurrentLayerRight)
            {
                m_isLastLayerLeft = isCurrentLayerLeft;
                m_isLastLayerRight = isCurrentLayerRight;
                
                direction.x *= -1;
                
                m_shootModel.m_bubbleMovingPositions.Add(m_bubblePosition);
                return RayCastToAim(m_aimViewController.m_BubblePosition, direction);
            }
            else
            {
                m_bubbleThrowPosition = GetBubbleThrowPosition();
                m_aimViewController.ShowBubbleAtThrowPosition(m_bubbleThrowPosition);
            }

            return true;
        }
        else
        {
            Hide();
        }

        return false;
    }

    private void Hide()
    {
        m_aimViewController.HideBubble();
    }

    private Vector3 GetBubbleThrowPosition()
    {
        int currentColumn = m_bubblePosition.x < -30 ? 1 : (Mathf.RoundToInt(Mathf.Abs(m_bubblePosition.x) / GameConstants.g_xSpawnDelta) + 1);

        int currentRow = m_bubblePosition.y > 0 ? 0 : (int) (Mathf.Floor(Mathf.Abs(m_bubblePosition.y) / GameConstants.g_ySpawnDelta) + 1);

        currentColumn = Mathf.Clamp(currentColumn, 0, GameConstants.g_maxColumns);
        currentRow = Mathf.Clamp(currentRow, 0, GameplayData.g_RowsOnScreenCount);

        m_gridPosition = (currentRow * GameConstants.g_maxColumns) + currentColumn;
        m_gridPosition--;

        bool isSlotAvailable = GamePlayController.Instance.IsGridSlotAvailable(m_gridPosition);
        bool hasRoof = m_gridPosition < GameConstants.g_maxColumns ? true : GamePlayController.Instance.HasRoof(m_gridPosition);

        int loopCount = 10;

        while (!(isSlotAvailable && hasRoof))
        {
            loopCount--;
            if (loopCount < 0)
            {
                Hide();
                return Vector3.zero;

            }
            if (m_gridPosition > 2 && (m_isLastLayerLeft && !isSlotAvailable) || (m_isLastLayerRight && !hasRoof))
                m_gridPosition--;
            else
                m_gridPosition++;

            isSlotAvailable = GamePlayController.Instance.IsGridSlotAvailable(m_gridPosition);
            hasRoof = GamePlayController.Instance.HasRoof(m_gridPosition);
        }

        currentRow = m_gridPosition / GameConstants.g_maxColumns;

        currentColumn = m_gridPosition % GameConstants.g_maxColumns;

        currentRow = Mathf.Clamp(currentRow, 0, GameplayData.g_RowsOnScreenCount);
        currentColumn = Mathf.Clamp(currentColumn, 0, GameConstants.g_maxColumns);

        bool isRowWithOffset = GameplayData.g_IsFirstRowWithOffset ? currentRow % 2 == 0 : currentRow % 2 == 1;

        float xPos = currentColumn * GameConstants.g_xSpawnDelta;
        float yPos = currentRow * -GameConstants.g_ySpawnDelta;

        if (isRowWithOffset)
        {
            xPos -= GameConstants.g_spawnDeltaOffset;
        }

        Vector3 shootPosition = new Vector3(xPos, yPos, 0);
        
        m_shootModel.m_column = currentColumn;
        m_shootModel.m_row = currentRow;
        m_shootModel.m_gridIndex = m_gridPosition;

        m_shootModel.m_bubbleMovingPositions.Add(shootPosition);
        
        return shootPosition;
    }

    #endregion Aiming
}
