using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{
    internal static class TableOperation<T> where T : ITable
    {
        static public Dictionary<int, T> identity_map = new Dictionary<int, T>();

        static public void RefreshAll(ref Dictionary<int, T> dict)
        {
            List<int> ids_to_refresh = new List<int>();

            var tmp_list = DBGateway<T>.ReadAll();
            
            foreach (var row in tmp_list)
            {
                if (dict.ContainsKey(row.Id.Value))
                {
                    if (row.LastUpdated <= dict[row.Id.Value].LastUpdated)
                    {
                        ids_to_refresh.Add(row.Id.Value);
                    }
                    else if (row.LastUpdated > dict[row.Id.Value].LastUpdated)
                    {   
                        dict[row.Id.Value] = row;
                        dict[row.Id.Value].MapRelations();
                    }
                }
                else
                {
                    dict[row.Id.Value] = row;
                }
            }

            foreach (var id in ids_to_refresh) 
            {
                dict[id] = DBGateway<T>.Update(dict[id]);
                dict[id].MapRelations();
            }
        }

        static public void RefreshAll()
        {
            List<int> ids_to_refresh = new List<int>();

            var tmp_list = DBGateway<T>.ReadAll();

            foreach (var row in tmp_list)
            {
                if (identity_map.ContainsKey(row.Id.Value))
                {
                    if (row.LastUpdated <= identity_map[row.Id.Value].LastUpdated)
                    {
                        ids_to_refresh.Add(row.Id.Value);
                    }
                    else if (row.LastUpdated > identity_map[row.Id.Value].LastUpdated)
                    {
                        identity_map[row.Id.Value] = row;
                        identity_map[row.Id.Value].MapRelations();
                    }
                }
                else
                {
                    identity_map[row.Id.Value] = row;
                }
            }

            foreach (var id in ids_to_refresh)
            {
                identity_map[id] = DBGateway<T>.Update(identity_map[id]);
                identity_map[id].MapRelations();
            }
        }

        static public void Update(ref Dictionary<int, T> dict, ref T entity)
        {
            var tmp = DBGateway<T>.Read(entity);
            
            if (tmp.LastUpdated <= entity.LastUpdated)
            {
                dict[entity.Id.Value] = DBGateway<T>.Update(entity);
                dict[entity.Id.Value].MapRelations();
            }
            else if (tmp.LastUpdated > entity.LastUpdated)
            {
                dict[entity.Id.Value] = tmp;
                dict[entity.Id.Value].MapRelations();
            }
        }

        static public void Update(ref T entity)
        {
            var tmp = DBGateway<T>.Read(entity);

            if (tmp.LastUpdated <= entity.LastUpdated)
            {
                identity_map[entity.Id.Value] = DBGateway<T>.Update(entity);
                identity_map[entity.Id.Value].MapRelations();
            }
            else if (tmp.LastUpdated > entity.LastUpdated)
            {
                identity_map[entity.Id.Value] = tmp;
                identity_map[entity.Id.Value].MapRelations();
            }
        }

        static public void Delete(ref Dictionary<int, T> dict, ref T entity) 
        {
            entity.RemoveSelfFromRelatives();
            dict.Remove(entity.Id.Value);
            DBGateway<T>.Delete(entity);
        }

        static public void Delete(ref T entity)
        {
            entity.RemoveSelfFromRelatives();
            identity_map.Remove(entity.Id.Value);
            DBGateway<T>.Delete(entity);
        }

        static public void DeleteSelection(ref Dictionary<int, T> dict, List<T> entities)
        {
            foreach (var entity in entities)
            {
                DBGateway<T>.Delete(entity);
                dict.Remove(entity.Id.Value);
            }
        }

        static public void DeleteSelection(List<T> entities)
        {
            foreach (var entity in entities)
            {
                DBGateway<T>.Delete(entity);
                identity_map.Remove(entity.Id.Value);
            }
        }

        static public void Create(ref Dictionary<int, T> dict, T entity)
        {
            entity = DBGateway<T>.Create(entity);
            dict[entity.Id.Value] = entity;
            dict[entity.Id.Value].MapRelations();
        }
        static public void Create(T entity)
        {
            entity = DBGateway<T>.Create(entity);
            identity_map[entity.Id.Value] = entity;
            identity_map[entity.Id.Value].MapRelations();
        }

        static public T GetRelated(int id)
        {
            if (identity_map.ContainsKey(id))
            {
                return identity_map[id];
            }
            else
            {
                RefreshAll(ref identity_map);
                return identity_map[id];
            }
        }

    }
}
