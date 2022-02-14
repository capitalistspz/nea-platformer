using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;

namespace common.entities
{
    public class EntityManager
    {
        private KeyedCollection<Guid, BaseEntity> _entities;
        private Queue<BaseEntity> _newEntities;
        private Queue<BaseEntity> _oldEntities;
        public EntityManager()
        {
            _entities = new KeyedCollection<Guid,BaseEntity>(e => e.Id);
            _newEntities = new Queue<BaseEntity>();
            _oldEntities = new Queue<BaseEntity>();
        }

        public void Update(GameTime time)
        {
            while (_oldEntities.Count > 0)
            {
                _entities.Remove(_oldEntities.Dequeue());
            }
            foreach (var entity in _entities.Values)
            {
                entity.Update(time);
            }
            while (_newEntities.Count > 0)
            {
                _entities.Add(_newEntities.Dequeue());
            }
            
        }

        public void RemoveEntity(BaseEntity entity)
        {
            _oldEntities.Enqueue(entity);
        }

        public void AddEntity(BaseEntity entity)
        {
            _newEntities.Enqueue(entity);
        }
        public BaseEntity GetEntity(Guid id)
        {
            return _entities[id];
        }

        public PlayerEntity GetPlayer(Guid id)
        {
            var entity = _entities[id];
            if (entity is PlayerEntity player)
                return player;
            else
                return null;
        }

        public List<BaseEntity> GetEntity(EntitySearch search)
        {
            return search.Invoke(_entities.Values);
        }
        public delegate List<BaseEntity> EntitySearch(ICollection<BaseEntity> entities);
    }
}