using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject playerShape;
    public GameObject outline;

    public GameManager gameManager;

    void CheckMatch()
    {
        // If player's shape matches the outline, call LevelComplete().
        if (IsShapeMatching())
        {
            gameManager.LevelComplete();
        }
    }

    bool IsShapeMatching()
    {
        // Get the PolygonCollider2D components from the playerShape and outline
        PolygonCollider2D playerShapeCollider = playerShape.GetComponent<PolygonCollider2D>();
        PolygonCollider2D outlineCollider = outline.GetComponent<PolygonCollider2D>();

        // Get the bounds of the playerShapeCollider and outlineCollider
        Bounds playerBounds = playerShapeCollider.bounds;
        Bounds outlineBounds = outlineCollider.bounds;

        // Check if the bounds of the playerShapeCollider and outlineCollider are approximately equal
        if (Mathf.Approximately(playerBounds.min.x, outlineBounds.min.x) &&
            Mathf.Approximately(playerBounds.min.y, outlineBounds.min.y) &&
            Mathf.Approximately(playerBounds.max.x, outlineBounds.max.x) &&
            Mathf.Approximately(playerBounds.max.y, outlineBounds.max.y))
        {
            // Use Physics2D.OverlapArea to check if the playerShapeCollider and outlineCollider are overlapping
            Collider2D[] results = new Collider2D[1];
            int overlapCount = Physics2D.OverlapArea(playerBounds.min, playerBounds.max, new ContactFilter2D().NoFilter(), results);

            // If there's at least one overlap and the first result is the outlineCollider, the shapes match
            if (overlapCount > 0 && results[0] == outlineCollider)
            {
                return true;
            }
        }

        return false;
    }

    void Update()
    {
        CheckMatch();
    }
}
