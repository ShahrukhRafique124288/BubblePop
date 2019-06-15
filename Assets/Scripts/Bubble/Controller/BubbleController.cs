using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BubbleController : GamePlayStateBaseController
{
    #region Varibales & Properties

    private BubbleViewController m_bubbleViewController;
    private BubbleViewRefs m_bubbleRefs;

    private Dictionary<int, BubbleModel> m_activeBubblesDict;
    private List<BubbleModel> m_inactiveBubblesList;
    private List<BubbleModel> m_bubblesToMerge = new List<BubbleModel>();

    private BubbleModel m_currentBubbleToShoot;
    private BubbleModel m_nextBubbleToShoot;

    private ShootModel m_shootModel;

    private int m_rowsCount;
    private int m_streakValue;

    private bool m_isFirstRowWithOffset;
    private bool m_isBubbleReachMax;

    private WaitForSeconds m_dropBubbleCRWait;
    private readonly float m_dropBubbleCRDelay = 1f;

    private WaitForSeconds m_destroyWait;
    private readonly float m_destroyCRDelay = 0.5f;

    private WaitForSeconds m_explodeWait;
    private readonly float m_explodeCRDelay = 1f;

    #endregion Varibales & Properties

    #region Initialization

    public BubbleController()
    {
        Initialize();

        PoolBubbles(50);
    }

    private void Initialize()
    {
        m_bubbleViewController = new BubbleViewController();

        m_bubbleRefs = GameRefs.Instance.m_bubbleRefs;

        m_activeBubblesDict = new Dictionary<int, BubbleModel>();
        m_inactiveBubblesList = new List<BubbleModel>();
        m_bubblesToMerge = new List<BubbleModel>();

        m_dropBubbleCRWait = new WaitForSeconds(m_dropBubbleCRDelay);
        m_destroyWait = new WaitForSeconds(m_destroyCRDelay);
        m_explodeWait = new WaitForSeconds(m_explodeCRDelay);
    }

    #endregion Initialization

    #region Game Flow

    public void StartGame()
    {
        SpawnInitialBubbles();
        SpawnCurrentBubble();
        SpawnNextBubble();
    }
    
    public void PauseGame()
    {
     
    }
    
    public void ResumeGame()
    {
     
    }

    #endregion Game Flow

    #region Pooling

    private void PoolBubbles(int poolSize)
    {
        GameObject loadedBubbleInMemory = Object.Instantiate(m_bubbleRefs.m_bubblePrefab, m_bubbleRefs.m_bubbleContainer);
        
        for (int index = 0; index < poolSize; index++)
        {
            GameObject bubble = Object.Instantiate(loadedBubbleInMemory, m_bubbleRefs.m_bubbleContainer);
            BubbleRefs bubbleRefs = bubble.GetComponent<BubbleRefs>();

            BubbleModel bubbleModel = new BubbleModel();
            bubbleModel.m_bubbleRefs = bubbleRefs;

            m_inactiveBubblesList.Add(bubbleModel);
        }
    }

    private BubbleModel GetBubbleModel()
    {
        if (m_inactiveBubblesList.Count == 0)
        {
            PoolBubbles(1);
        }

        BubbleModel bubbleModel = m_inactiveBubblesList[0];
        m_inactiveBubblesList.RemoveAt(0);
        ResetBubble(bubbleModel);
        return bubbleModel;
    }

    private void ResetBubble(BubbleModel bubbleModel)
    {
        bubbleModel.m_bubbleRefs.m_rigidbody.gravityScale = 0;
        bubbleModel.m_bubbleRefs.m_rigidbody.velocity = Vector2.zero;

        bubbleModel.m_bubbleRefs.m_bubbleTransform.localScale = Vector3.zero;
        bubbleModel.m_bubbleRefs.m_bubbleTransform.localPosition = Vector3.zero;

        Color tempColor = Color.white;
        tempColor.a = 1;

        bubbleModel.m_bubbleRefs.m_bubbleBg.color = tempColor;
        bubbleModel.m_bubbleRefs.m_valueText.color = tempColor;

        bubbleModel.m_bubbleRefs.m_bubbleBg.transform.localScale = Vector3.one * 0.6f;
    }

    private void AddToInactiveBubbleList(BubbleModel bubbleModel)
    {
        if (m_inactiveBubblesList.Contains(bubbleModel))
            return;

        ResetBubble(bubbleModel);

        m_inactiveBubblesList.Add(bubbleModel);
    }

    private void PoolDestroyedBubbles(List<BubbleModel> bubbleModels)
    {
        for (int i = 0; i < bubbleModels.Count; i++)
        {
            AddToInactiveBubbleList(bubbleModels[i]);
        }

        bubbleModels.Clear();
    }

    #endregion Pooling

    #region Spawning

    private void SpawnInitialBubbles()
    {
        UpdateRowCount(GameConstants.g_rowsToStart);

        m_isFirstRowWithOffset = false;
        GameplayData.g_IsFirstRowWithOffset = m_isFirstRowWithOffset;

        int bubblesToSpawn = m_rowsCount * GameConstants.g_maxColumns;

        for (int index = 0; index < bubblesToSpawn; index++)
        {
            SpawnBubble(index, true);
        }
    }

    private void SpawnBubble(int index, bool isInitialSpawning = false)
    {
        BubbleModel bubbleModel = GetBubbleModel();
        bubbleModel.m_gridIndex = index;
        bubbleModel.m_value = GetBubbleValue();
        bubbleModel.m_bubbleColor = GetBubbleColor(bubbleModel.m_value);

        int currentRow = index / GameConstants.g_maxColumns;
        int currentColumn = index % GameConstants.g_maxColumns;
        
        float xPos = currentColumn * GameConstants.g_xSpawnDelta;
        float yPos = currentRow * -GameConstants.g_ySpawnDelta;
        
        bool isRowWithOffset = m_isFirstRowWithOffset ? currentRow % 2 == 0 : currentRow % 2 == 1;

        xPos = isRowWithOffset ? xPos - GameConstants.g_spawnDeltaOffset : xPos;

        bubbleModel.m_position = new Vector2(xPos, yPos);

        m_activeBubblesDict.Add(index, bubbleModel);

        m_bubbleViewController.SpawnBubble(bubbleModel, 1, isInitialSpawning);
    }

    private void SpawnCurrentBubble()
    {
        BubbleModel bubbleModel = GetBubbleModel();
        bubbleModel.m_value = GetBubbleValue();
        bubbleModel.m_bubbleColor = GetBubbleColor(bubbleModel.m_value);
        bubbleModel.m_position = m_bubbleRefs.m_currentBubblePosition.localPosition;
        bubbleModel.m_bubbleRefs.m_circleCollider.enabled = false;

        m_currentBubbleToShoot = bubbleModel;

        m_bubbleViewController.SpawnBubble(m_currentBubbleToShoot);

        GameplayData.g_currentBubbleColor = m_currentBubbleToShoot.m_bubbleColor;
    }
    
    private void SpawnNextBubble()
    {
        BubbleModel bubbleModel = GetBubbleModel();
        bubbleModel.m_value = GetBubbleValue();
        bubbleModel.m_bubbleColor = GetBubbleColor(bubbleModel.m_value);
        bubbleModel.m_position = m_bubbleRefs.m_nextBubblePosition.localPosition;
        bubbleModel.m_bubbleRefs.m_circleCollider.enabled = false;

        m_nextBubbleToShoot = bubbleModel;

        m_bubbleViewController.SpawnBubble(m_nextBubbleToShoot, 0.75f);
        
    }

    #endregion Spawning

    #region Value Management

    private int GetBubbleValue()
    {
        return GameConstants.g_bubbleValues[Random.Range(0, GameConstants.g_bubbleValues.Length)];
    }

    private Color GetBubbleColor(int bubbleValue)
    {
        int colorIndex = Mathf.RoundToInt(Mathf.Log(bubbleValue, 2)) - 1;
        colorIndex--;

        if (colorIndex >= ColorsData.Instance.m_bubbleColors.Length)
            colorIndex = ColorsData.Instance.m_bubbleColors.Length - 1;

        return ColorsData.Instance.m_bubbleColors[colorIndex];
    }

    public bool IsGridSlotAvailable(int index)
    {
        return !m_activeBubblesDict.ContainsKey(index);
    }

    public bool HasRoof(int index)
    {
        bool hasRoof = true;
        
        int upperIndex = index - GameConstants.g_maxColumns;

        if (upperIndex >= 0)
        {
            hasRoof = m_activeBubblesDict.ContainsKey(upperIndex);

            if (!hasRoof)
            {
                int currentIndexRow = index / GameConstants.g_maxColumns;

                int upperIndexRow = upperIndex / GameConstants.g_maxColumns;
                bool isRowWithOffset = m_isFirstRowWithOffset ? upperIndexRow % 2 == 0 : upperIndexRow % 2 == 1;

                int nextRow = (index + 1) / GameConstants.g_maxColumns;
                hasRoof |= (nextRow == currentIndexRow && m_activeBubblesDict.ContainsKey(index + 1));
                
                nextRow = (index - 1) / GameConstants.g_maxColumns;
                hasRoof |= (nextRow == currentIndexRow && m_activeBubblesDict.ContainsKey(index - 1));

                if (isRowWithOffset)
                {
                    hasRoof |= m_activeBubblesDict.ContainsKey(upperIndex + 1);
                }
                else
                {
                    hasRoof |= m_activeBubblesDict.ContainsKey(upperIndex - 1);
                }
            }
        }

        return hasRoof;
    }

    #endregion Value Management

    #region Rows Management

    private void UpdateRows()
    {
        int currentRows = (m_activeBubblesDict.Keys.Max() / GameConstants.g_maxColumns) + 1;

        UpdateRowCount(currentRows);

        if (m_rowsCount < GameConstants.g_minRowsOnScreen)
        {
            AddRow();
        }
        else if (m_rowsCount > GameConstants.g_maxRows)
        {
            RemoveRow();
        }
    }

    private void AddRow()
    {
        UpdateOffsetRow();

        UpdateRowCount(m_rowsCount + 1);

        Dictionary<int, BubbleModel> updatedBubblePositions = new Dictionary<int, BubbleModel>();

        foreach (KeyValuePair<int, BubbleModel> bubbleModel in m_activeBubblesDict)
        {
            int newKey = bubbleModel.Key + GameConstants.g_maxColumns;
            bubbleModel.Value.m_gridIndex = newKey;
            updatedBubblePositions.Add(newKey, bubbleModel.Value);

            Vector3 bubbleNewPosition = bubbleModel.Value.m_bubbleRefs.transform.localPosition + (Vector3.down * GameConstants.g_ySpawnDelta);

            m_bubbleViewController.MoveBubbleToNewPosition(bubbleModel.Value.m_bubbleRefs.transform, bubbleNewPosition);
        }

        m_activeBubblesDict.Clear();
        m_activeBubblesDict.AddDictionaryElementInDictionary(updatedBubblePositions);

        for (int index = 0; index < GameConstants.g_maxColumns; index++)
        {
            SpawnBubble(index);
        }
    }

    private void RemoveRow()
    {
        UpdateOffsetRow();
        UpdateRowCount(m_rowsCount - 1);

        Dictionary<int, BubbleModel> updatedBubblePositions = new Dictionary<int, BubbleModel>();

        foreach (KeyValuePair<int, BubbleModel> bubbleModel in m_activeBubblesDict)
        {
            int newKey = bubbleModel.Key - GameConstants.g_maxColumns;

            Vector3 bubbleNewPosition = bubbleModel.Value.m_bubbleRefs.transform.localPosition + (Vector3.up * GameConstants.g_ySpawnDelta);

            m_bubbleViewController.MoveBubbleToNewPosition(bubbleModel.Value.m_bubbleRefs.transform, bubbleNewPosition);

            if (newKey < 0)
            {
                AddToInactiveBubbleList(bubbleModel.Value);
            }
            else
            {
                bubbleModel.Value.m_gridIndex = newKey;
                updatedBubblePositions.Add(newKey, bubbleModel.Value);
            }
        }

        m_activeBubblesDict.Clear();
        m_activeBubblesDict.AddDictionaryElementInDictionary(updatedBubblePositions);
    }

    private void UpdateOffsetRow()
    {
        m_isFirstRowWithOffset = !m_isFirstRowWithOffset;
        GameplayData.g_IsFirstRowWithOffset = m_isFirstRowWithOffset;
    }

    private void UpdateRowCount(int count)
    {
        m_rowsCount = count;
        GameplayData.g_RowsOnScreenCount = m_rowsCount;
    }

    #endregion Rows Management

    #region Shoot Mechanics

    public void BubbleShot(ShootModel shootModel)
    {
        m_streakValue = 1;
        m_shootModel = shootModel;
        m_currentBubbleToShoot.m_gridIndex = m_shootModel.m_gridIndex;
        m_currentBubbleToShoot.m_bubbleRefs.m_circleCollider.enabled = true;
        SetTrailState(true);

        m_bubbleViewController.MoveBubbleToTarget(m_currentBubbleToShoot.m_bubbleRefs.m_bubbleTransform, m_shootModel.m_bubbleMovingPositions, BubbleTargetReached);
    }

    private void BubbleTargetReached()
    {
        EventManager.DoFireShakeBoardEvent(false);
        Vibration.Vibrate(TapticPlugin.ImpactFeedback.Heavy);
        SetTrailState(false);
        m_currentBubbleToShoot.m_bubbleRefs.m_trail.time = 0;
        MergeBubble();
    }

    #endregion Shoot Mechanics

    #region Trail

    private void SetTrailState(bool state)
    {
        m_currentBubbleToShoot.m_bubbleRefs.m_trail.enabled = state;
    }

    #endregion Trail

    #region Merge

    private void MergeBubble()
    {
        List<BubbleModel> bubblesToMerge = new List<BubbleModel>();
        m_bubblesToMerge.Clear();

        bubblesToMerge.Add(m_currentBubbleToShoot);

        PerformMerge(bubblesToMerge, m_currentBubbleToShoot.m_gridIndex, m_currentBubbleToShoot.m_value);

        if (bubblesToMerge.Count == 1)
        {
            m_activeBubblesDict.Add(m_currentBubbleToShoot.m_gridIndex, m_currentBubbleToShoot);
            UpdateStateToShootAgain();
        }
        else
        {
            int mergeValue = GetMergeValue(bubblesToMerge.Count, m_currentBubbleToShoot.m_value);
            mergeValue = Mathf.Clamp(mergeValue, 2, GameConstants.g_bubbleBlastValue);

            m_isBubbleReachMax = mergeValue >= GameConstants.g_bubbleBlastValue;

            SortBubblesList(bubblesToMerge);

            int mergePoint = m_isBubbleReachMax ? 0 : GetNextMergeIndex(bubblesToMerge, mergeValue);

            m_currentBubbleToShoot = bubblesToMerge[mergePoint];

            m_currentBubbleToShoot.m_value = mergeValue;
            m_currentBubbleToShoot.m_bubbleColor = GetBubbleColor(mergeValue);

            if (m_activeBubblesDict.Count == 0)
            {
                m_bubbleViewController.BubbleDrop(m_currentBubbleToShoot);
            }

            m_bubbleViewController.MoveBubbleToMerge(bubblesToMerge, mergePoint, MergeComplete);
            DropDisconnectedBubbles();
            m_bubblesToMerge.AddRange(bubblesToMerge);

            Vibration.Vibrate(TapticPlugin.ImpactFeedback.Heavy);
        }
    }

    private void UpdateStateToShootAgain()
    {
        UpdateRows();

        UpdateNextShootBubble();

        DropDisconnectedBubbles();
    }

    private void UpdateNextShootBubble()
    {
        m_currentBubbleToShoot = m_nextBubbleToShoot;
        m_currentBubbleToShoot.m_position = m_bubbleRefs.m_currentBubblePosition.localPosition;
        GameplayData.g_currentBubbleColor = m_currentBubbleToShoot.m_bubbleColor;

        m_bubbleViewController.SetCurrentBubble(m_currentBubbleToShoot);
        SpawnNextBubble();

        EventManager.DoFireMergeCompleteEvent();
    }

    private void MergeComplete()
    {
        m_bubblesToMerge.Remove(m_currentBubbleToShoot);
        PoolDestroyedBubbles(m_bubblesToMerge);

        SetPropertiesOfMergedBubble();

        EventManager.DoFireUpdateMergeValueEvent(m_currentBubbleToShoot.m_value);

        if (m_streakValue > 1)
        {
            EventManager.DoFireShowStreakValueEvent(m_streakValue);
        }

        m_streakValue++;

        if (m_activeBubblesDict.Count == 0)
        {
            PerfectHit();
        }
        else if (m_isBubbleReachMax)
        {
            ExplodeSurrounding();            
        }
        else
        {
            MergeBubble();
        }
    }

    private void SetPropertiesOfMergedBubble()
    {
        string bubbleValue = m_currentBubbleToShoot.m_value > 2000 ? "2K" : m_currentBubbleToShoot.m_value > 1000 ? "1K" : m_currentBubbleToShoot.m_value.ToString();
        m_currentBubbleToShoot.m_bubbleRefs.m_valueText.text = bubbleValue;
        m_currentBubbleToShoot.m_bubbleRefs.m_mergeText.text = bubbleValue;
        m_currentBubbleToShoot.m_bubbleRefs.m_bubbleBg.color = m_currentBubbleToShoot.m_bubbleColor;
        m_bubbleViewController.ShowMergedValue(m_currentBubbleToShoot);
    }

    private void SortBubblesList(List<BubbleModel> bubblesToMerge)
    {
        for (int i = 0; i <= bubblesToMerge.Count - 2; i++)
        {
            for (int j = 0; j <= bubblesToMerge.Count - 2; j++)
            {
                if (bubblesToMerge[j].m_gridIndex > bubblesToMerge[j + 1].m_gridIndex)
                {
                    BubbleModel temp = bubblesToMerge[j + 1];
                    bubblesToMerge[j + 1] = bubblesToMerge[j];
                    bubblesToMerge[j] = temp;
                }
            }
        }
    }

    private int GetNextMergeIndex(List<BubbleModel> bubblesToMerge,int value)
    {
        int nextMergeIndex = 0;

        for (int i = 0; i < bubblesToMerge.Count; i++)
        {
            int currentIndex = bubblesToMerge[i].m_gridIndex;
            if (IsNextMergePossible(currentIndex, value, -GameConstants.g_maxColumns, true))
            {
                nextMergeIndex = i;
                break;
            }
            if (IsNextMergePossible(currentIndex, value, -1, false))
            {
                nextMergeIndex = i;
                break;
            }
            if (IsNextMergePossible(currentIndex, value, 1, false))
            {
                nextMergeIndex = i;
                break;
            }
            if (IsNextMergePossible(currentIndex, value, GameConstants.g_maxColumns, true))
            {
                nextMergeIndex = i;
                break;
            }
        }

        return nextMergeIndex;
    }

    private bool IsNextMergePossible(int currentIndex, int value, int nextIndexDelta, bool isVerticalMerge)
    {
        int nextIndex = currentIndex + nextIndexDelta;
        int nextIndexRow = nextIndex / GameConstants.g_maxColumns;
        bool isRowWithOffset = m_isFirstRowWithOffset ? nextIndexRow % 2 == 0 : nextIndexRow % 2 == 1;

        if (!isVerticalMerge)
        {
            int currentIndexRow = currentIndex / GameConstants.g_maxColumns;
            if (nextIndexRow != currentIndexRow)
                return false;
        }

        if (m_activeBubblesDict.ContainsKey(nextIndex) && m_activeBubblesDict[nextIndex].m_value == value)
        {
            return true;
        }

        if (isVerticalMerge)
        {
            int nextIndexOffset = isRowWithOffset ? 1 : -1;
            nextIndex += nextIndexOffset;

            if (nextIndexRow == nextIndex / GameConstants.g_maxColumns && m_activeBubblesDict.ContainsKey(nextIndex) && m_activeBubblesDict[nextIndex].m_value == value)
            {
                return true;
            }
        }

        return false;
    }

    private void PerfectHit()
    {
        EventManager.DoFirePerfectShootEvent();
        AddToInactiveBubbleList(m_currentBubbleToShoot);
        SpawnInitialBubbles();
        UpdateNextShootBubble();
        Vibration.Vibrate(TapticPlugin.ImpactFeedback.Heavy);
    }

    private void ExplodeSurrounding()
    {
        if (!m_activeBubblesDict.ContainsKey(m_currentBubbleToShoot.m_gridIndex))
            m_activeBubblesDict.Add(m_currentBubbleToShoot.m_gridIndex, m_currentBubbleToShoot);

        List<int> explodingIndexes = new List<int>();

        int currentIndex = m_currentBubbleToShoot.m_gridIndex + (GameConstants.g_maxColumns * 3);
        int currentRow = currentIndex / GameConstants.g_maxColumns;
        int indexDelta = 0;
        int indexesToAdd = 8;

        GetExplodingGridIndexes(explodingIndexes, indexesToAdd, currentIndex, currentRow, indexDelta);

        List<BubbleModel> bubbleToExplode = new List<BubbleModel>();

        for (int i = 0; i < explodingIndexes.Count; i++)
        {
            int gridIndex = explodingIndexes[i];
            if (m_activeBubblesDict.ContainsKey(gridIndex))
            {
                bubbleToExplode.Add(m_activeBubblesDict[gridIndex]);
                m_activeBubblesDict.Remove(gridIndex);
            }
        }

        CoRoutineRunner.Instance.StartCoroutine(ExplodeSurroundingCR(bubbleToExplode));

        EventManager.DoFireShakeBoardEvent(true);
        Vibration.Vibrate(TapticPlugin.ImpactFeedback.Heavy);
    }

    private IEnumerator ExplodeSurroundingCR(List<BubbleModel> bubbleToExplode)
    {
        foreach (BubbleModel bubbleModel in bubbleToExplode)
        {
            bubbleModel.m_bubbleRefs.m_rigidbody.AddForce(new Vector2(Random.Range(-250, 250), Random.Range(-120, 120)));
            m_bubbleViewController.ExplodeBubble(bubbleModel);
        }

        DropDisconnectedBubbles();

        yield return m_explodeWait;

        UpdateStateToShootAgain();

        for (int i = 0; i < bubbleToExplode.Count; i++)
        {
            AddToInactiveBubbleList(bubbleToExplode[i]);
        }

        bubbleToExplode.Clear();

    }

    private void GetExplodingGridIndexes(List<int> explodingIndexes, int indexesToAdd, int currentIndex,int currentRow, int indexDelta)
    {
        if (currentIndex < 0)
            return;

        indexesToAdd--;
        indexDelta++;

        if (indexesToAdd <= 0)
            return;

        currentIndex -= GameConstants.g_maxColumns;
        currentRow = currentIndex / GameConstants.g_maxColumns;

        GetExplodingGridIndexes(explodingIndexes, indexesToAdd, currentIndex, currentRow, indexDelta);

        int surroundingIndexesToAdd = indexesToAdd > 4 ? indexesToAdd - (indexesToAdd - indexDelta) : indexesToAdd - 1;

        explodingIndexes.Add(currentIndex);

        if (surroundingIndexesToAdd > 1)
        {
            int leftHalf = surroundingIndexesToAdd / 2;
            for (int i = 0; i < leftHalf; i++)
            {
                int nextIndex = currentIndex - 1;
                int nextIndexRow = nextIndex / GameConstants.g_maxColumns;

                if (currentRow == nextIndexRow)
                    explodingIndexes.Add(nextIndex);
                else break;
            }

            int rightHalf = surroundingIndexesToAdd - leftHalf;
            for (int i = 0; i < rightHalf; i++)
            {
                int nextIndex = currentIndex - 1;
                int nextIndexRow = nextIndex / GameConstants.g_maxColumns;

                if (currentRow == nextIndexRow)
                    explodingIndexes.Add(nextIndex);
                else break;
            }
        }
        else if (surroundingIndexesToAdd > 0)
        {
            int nextIndex = currentIndex - 1;
            int nextIndexRow = nextIndex / GameConstants.g_maxColumns;

            if (currentRow != nextIndexRow)
            {
                nextIndex = currentIndex + 1;
                nextIndexRow = nextIndex / GameConstants.g_maxColumns;

                if (currentRow == nextIndexRow)
                    explodingIndexes.Add(nextIndex);
            }
            else
                explodingIndexes.Add(nextIndex);
        }
    }

    private void DropDisconnectedBubbles()
    {
        if (m_activeBubblesDict.Count == 0)
            return;

        int lastGridValue = m_activeBubblesDict.Keys.Max();
        int lastRow = lastGridValue / GameConstants.g_maxColumns;

        List<BubbleModel> disconnectedBubbles = new List<BubbleModel>();

        for (int gridIndex = lastGridValue; gridIndex >= GameConstants.g_maxColumns; gridIndex--)
        {
            if (m_activeBubblesDict.ContainsKey(gridIndex))
            {
                if (lastRow != gridIndex / GameConstants.g_maxColumns && disconnectedBubbles.Count > 0)
                {
                    DropBubbles(disconnectedBubbles);
                    break;
                }

                int topIndex = gridIndex - GameConstants.g_maxColumns;

                int topIndexRow = topIndex / GameConstants.g_maxColumns;
                bool isRowWithOffset = m_isFirstRowWithOffset ? topIndexRow % 2 == 0 : topIndexRow % 2 == 1;
                int nextIndexOffset = isRowWithOffset ? 1 : -1;
                int nextToTopIndex = topIndex + nextIndexOffset;

                if (!m_activeBubblesDict.ContainsKey(topIndex) && m_currentBubbleToShoot.m_gridIndex != topIndex
                    && !(topIndexRow == nextToTopIndex / GameConstants.g_maxColumns && m_activeBubblesDict.ContainsKey(nextToTopIndex)) && m_currentBubbleToShoot.m_gridIndex != nextToTopIndex)
                {
                    disconnectedBubbles.Add(m_activeBubblesDict[gridIndex]);
                    m_activeBubblesDict.Remove(gridIndex);
                }
                else
                {
                    for (int i = 0; i < disconnectedBubbles.Count; i++)
                    {
                        m_activeBubblesDict.Add(disconnectedBubbles[i].m_gridIndex, disconnectedBubbles[i]);
                    }

                    disconnectedBubbles.Clear();
                }
            }
            else if (disconnectedBubbles.Count > 0)
            {
                DropBubbles(disconnectedBubbles);
                break;
            }
        }
    }

    private void DropBubbles(List<BubbleModel> disconnectedBubbles)
    {
        CoRoutineRunner.Instance.StartCoroutine(DropBubblesCR(disconnectedBubbles));
        DropDisconnectedBubbles();
    }

    private IEnumerator DropBubblesCR(List<BubbleModel> disconnectedBubbles)
    {
        foreach (BubbleModel bubbleModel in disconnectedBubbles)
        {
            bubbleModel.m_bubbleRefs.m_rigidbody.AddForce(new Vector2(Random.Range(-70, 70), Random.Range(10, 80)));
            bubbleModel.m_bubbleRefs.m_rigidbody.gravityScale = 1f;
        }

        yield return m_dropBubbleCRWait;

        foreach (BubbleModel bubbleModel in disconnectedBubbles)
        {
            bubbleModel.m_bubbleRefs.m_rigidbody.AddForce(new Vector2(Random.Range(-10, 10), Random.Range(20, 50)));
            m_bubbleViewController.BubbleDrop(bubbleModel);
        }

        Vibration.Vibrate(TapticPlugin.ImpactFeedback.Medium);

        yield return m_destroyWait;

        EventManager.DoFireShakeBoardEvent(false);

        for (int i = 0; i < disconnectedBubbles.Count; i++)
        {
            AddToInactiveBubbleList(disconnectedBubbles[i]);
        }

        disconnectedBubbles.Clear();
    }

    private int GetMergeValue(int bubblesToMerge,int startValue)
    {
        int mergeValue = startValue;

        for (int i = 1; i < bubblesToMerge; i++)
        {
            mergeValue *= 2;
        }

        return mergeValue;
    }

    private void PerformMerge(List<BubbleModel> bubblesToMerge, int currentIndex, int value)
    {
        CheckForMerge(bubblesToMerge, currentIndex, value, GameConstants.g_maxColumns, true);
        CheckForMerge(bubblesToMerge, currentIndex, value, -1, false);
        CheckForMerge(bubblesToMerge, currentIndex, value, 1, false);
        CheckForMerge(bubblesToMerge, currentIndex, value, -GameConstants.g_maxColumns, true);
    }

    private void CheckForMerge(List<BubbleModel> bubblesToMerge, int currentIndex, int value, int nextIndexDelta, bool isVerticalMerge)
    {
        int nextIndex = currentIndex + nextIndexDelta;
        int nextIndexRow = nextIndex / GameConstants.g_maxColumns;
        bool isRowWithOffset = m_isFirstRowWithOffset ? nextIndexRow % 2 == 0 : nextIndexRow % 2 == 1;

        if (!isVerticalMerge)
        {
            int currentIndexRow = currentIndex / GameConstants.g_maxColumns;
            if (nextIndexRow != currentIndexRow)
                return;
        }
        
        if (m_activeBubblesDict.ContainsKey(nextIndex) && m_activeBubblesDict[nextIndex].m_value == value)
        {
            bubblesToMerge.Add(m_activeBubblesDict[nextIndex]);
            m_activeBubblesDict.Remove(nextIndex);
            PerformMerge(bubblesToMerge, nextIndex, value);
        }

        if (isVerticalMerge)
        {
            int nextIndexOffset = isRowWithOffset ? 1 : -1;
            nextIndex += nextIndexOffset;

            if (nextIndexRow == nextIndex / GameConstants.g_maxColumns && m_activeBubblesDict.ContainsKey(nextIndex) && m_activeBubblesDict[nextIndex].m_value == value)
            {
                bubblesToMerge.Add(m_activeBubblesDict[nextIndex]);
                m_activeBubblesDict.Remove(nextIndex);
                PerformMerge(bubblesToMerge, nextIndex, value);
            }
        }
    }

    #endregion Merge
}
