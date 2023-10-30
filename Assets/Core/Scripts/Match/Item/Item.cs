using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace Match3
{
    public class Item
    {
        #region COMPONENTS

        protected SpriteRenderer spriteRenderer;
        #endregion
        
        #region VARIABLES

        private List<ISkill> skills;
        #endregion
        
        #region PROPERTIES

        public string Name { get; private set; }
        public GameObject GameObject { get; private set; }
        public Transform Transform { get; private set; }
        public string Id { get; private set; }
        public Cell Cell { get; private set; }
        public Cell FlowTarget { get; private set; }
        public bool CanFall { get; private set; }
        public float Velocity { get; set; }
        public bool IsFalling { get; set; }


        #endregion

        public Item(string id, string name, GameObject gameObject, List<ISkill> skills, bool canFall)
        {
            Id = id;
            Name = name;
            GameObject = gameObject;
            Transform = gameObject.transform;
            spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
            this.skills = skills;
            CanFall = canFall;
            foreach (var skill in this.skills)
            {
                skill.SetItem(this);
            }
        }

        public void SetSortingOrder(int indexY)
        {
            spriteRenderer.sortingOrder = indexY;
        }

        public void SetCell(Cell cell)
        {
            Cell = cell;
            cell.SetItem(this);
        }

        public void LeaveCell()
        {
            if (Cell == null) return;
            Cell.SetItem(null);
            Cell = null;
        }
        
        public bool TryGetSkill<T>( out T skill) where T : class, ISkill
        {
            foreach (var s in skills)
            {
                if (s is T desiredSkill)
                {
                    skill = desiredSkill;
                    return true;
                }
            }

            skill = null;
            return false;
        }
        

        public virtual async void Despawn()
        {
            spriteRenderer.enabled = false;
            Cell.SetItem(null);
            var destroyParticleFx = GameObject.GetComponentInChildren<ParticleSystem>();
            if (destroyParticleFx != null)
            {
                destroyParticleFx.Play();
                await Waiter.WaitForSeconds(1);
            }
            
            GameObject.Destroy(GameObject);
        }

        public void SetFlowTarget(Cell cell)
        {
            FlowTarget = cell;
            if (cell != null)
            {
                cell.IncomingItem = this;
            }
        }

        public virtual void DespawnSilent()
        {
            Cell.SetItem(null);
            GameObject.Destroy(GameObject);
        }

        public void PlayInteractAnimation()
        {
            GameObject.transform.DOShakeRotation(0.15f, new Vector3(0, 0, 20), fadeOut:true);
        }
    }
}