using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace  Match3
{
    public class TNTItem : Item
    {
        #region VARIABLES

        private TNTVisuals tntVisuals;
        private GameObject bombPrefab;
        #endregion
        
        public TNTItem(string id, string name, GameObject gameObject, GameObject bombPrefab, List<ISkill> skills, bool canFall) : base(id, name, gameObject, skills, canFall)
        {
            this.bombPrefab = bombPrefab;
            tntVisuals = gameObject.GetComponent<TNTVisuals>();
        }

        public async void Explode(int impactRadius)
        {
             var visuals = GameObject.GetComponent<TNTVisuals>();
             visuals.StopMergeReadyAnimation();
            
            Main.Instance.MatchCameraTransform.DOShakePosition(0.5f, 0.1f);
            var explosion = GameObject.Instantiate(bombPrefab, Cell.Transform);
            spriteRenderer.enabled = false;
            Cell.SetItem(null);
            var destroyParticleFx = GameObject.GetComponentInChildren<ParticleSystem>();
            if (destroyParticleFx != null)
            {
                destroyParticleFx.Play();
            }
            
            BlastNeighbors(impactRadius);
            
            await Waiter.WaitForSeconds(2f);
            GameObject.Destroy(explosion);
            GameObject.Destroy(GameObject);
        }

        public override void Despawn()
        {
            Explode(3);
            base.Despawn();
        }
        

        private async void BlastNeighbors(int impactRadius)
        {
            HashSet<Vector2Int> hitCells = new HashSet<Vector2Int>();

            for (int i = 0; i < impactRadius; i++)
            {
                var impactedCells = MatchManager.Instance.Board.GetCellsAtRadius(Cell.Index, i);
                foreach (var cell in impactedCells)
                {
                    if(hitCells.Contains(cell.Index)) continue;
                    cell.ToggleFlow(false);
                    hitCells.Add(cell.Index);
                    if (cell.Item != null && cell.Item.TryGetSkill(out BlastSkill blastSkill))
                    {
                        blastSkill.Blast(BlastType.STRONG);
                        // cell.Item.DespawnSilent();
                    }
                }

                await Waiter.WaitForSeconds(0.15f);
            }

            foreach (var index in hitCells)
            {
                if(MatchManager.Instance.Board.TryGetCell(index, out Cell cell))
                {
                    cell.ToggleFlow(true);
                }
            }
        }

        public void StartMergeReadyAnimation()
        {
            tntVisuals.SetSortingOrder(Cell.Index.y);
            tntVisuals.StartMergeReadyAnimation();
        }

        public void StopMergeReadyAnimation()
        {
            tntVisuals.StopMergeReadyAnimation();
        }
    }
}