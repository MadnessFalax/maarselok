using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{

    // currently there is race condition bug, when one client deletes a row while other client has the row loaded... when other client modifies/reads delete row an exception is raised
    // selecting before modifying and checking whether at least one result was queried might fix the issue
    public class DBGateway<T> where T : ITable
    {
        static public string connectionString = "";

        static public void setConnectionString(string connectionString)
        {
            DBGateway<T>.connectionString = connectionString;        
        }

        static public void Create(T entity)
        { 

            var tableName = typeof(T).Name;
            var field_dict = new Dictionary<string, KeyValuePair<Type, object>>();
            var insert_dict = new Dictionary<string, KeyValuePair<Type, object>>();
            var field_collection = typeof(T).GetProperties();

            foreach (var field in field_collection)
            {
                if (field.PropertyType == typeof(int?) || field.PropertyType == typeof(string) || field.PropertyType == typeof(DateTime?))
                {
                    field_dict[field.Name] = new KeyValuePair<Type, object>(field.PropertyType, field.GetValue(entity));
                    insert_dict[field.Name] = new KeyValuePair<Type, object>(field.PropertyType, field.GetValue(entity));
                }
            }

            if (insert_dict.ContainsKey("Id"))
            {
                insert_dict.Remove("Id");
            }

            if (insert_dict.ContainsKey("Created"))
            {
                insert_dict.Remove("Created");
            }

            if (insert_dict.ContainsKey("LastUpdated"))
            {
                insert_dict.Remove("LastUpdated");
            }


            int? id = null;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using var transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable); 
                try
                {
                    using var cmd = connection.CreateCommand();

                    StringBuilder fields = new StringBuilder();
                    StringBuilder values = new StringBuilder();
                
                    foreach (var field in insert_dict)
                    {
                        fields.Append($"{field.Key.ToString()},");
                        if (field.Value.Key == typeof(string))
                        {
                            values.Append($"'{field.Value.Value.ToString()}',");
                        }
                        else if(field.Value.Key == typeof(DateTime?))
                        {
                            values.Append($"{((DateTime)field.Value.Value).ToString("yyyy-MM-ddTHH:mm:ss")}");
                        }
                        else if(field.Value.Key == typeof(int?))
                        {
                            values.Append($"{((int?)field.Value.Value).Value.ToString()},");
                        }

                    }

                    // remove trailing ','
                    values.Length--;
                    fields.Length--;

                    cmd.CommandText = $"insert into {tableName} ({fields.ToString()}) values ({values.ToString()})";

                    //cmd.Parameters.AddWithValue("@fields", fields.ToString());
                    //cmd.Parameters.AddWithValue("@values", fields.ToString());

                    cmd.ExecuteNonQuery();

                    /*
                    using var select = connection.CreateCommand();
                    select.CommandText = $"select * from {tableName} where (Id = @id)";
                    select.Parameters.AddWithValue("id", id);
                    using var reader = select.ExecuteReader();

                    T new_entity = (T) typeof(T).GetConstructor(new Type[0]).Invoke(null);

                    while(reader.Read())
                    {
                        foreach (var field in field_dict)
                        {
                            var name = field.Key;
                            if (field.Value.Key == typeof(string))
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal(name)))
                                {
                                    var value = reader.GetString(reader.GetOrdinal(name));
                                    typeof(T).GetProperty(name).SetValue(new_entity, value);
                                }
                                else
                                {
                                    var value = (string?) null;
                                    typeof(T).GetProperty(name).SetValue(new_entity, value);
                                }
                            }
                            else if (field.Value.Key == typeof(int?))
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal(name)))
                                {
                                    var value = (int)reader.GetInt64(reader.GetOrdinal(name));
                                    typeof(T).GetProperty(name).SetValue(new_entity, value);
                                }
                                else
                                {
                                    var value = (int?) null;
                                    typeof(T).GetProperty(name).SetValue(new_entity, value);
                                }
                            }
                            else if (field.Value.Key == typeof(DateTime?))
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal(name)))
                                {
                                    var value = (DateTime)reader.GetDateTime(reader.GetOrdinal(name));
                                    typeof(T).GetProperty(name).SetValue(new_entity, value);
                                }
                                else
                                {
                                    var value = (DateTime?)null;
                                    typeof(T).GetProperty(name).SetValue(new_entity, value);
                                }
                            }
                            else
                            {
                                throw new Exception("Invalid type?");
                            }
                        }
                    }
                    */
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        static public void Delete(T entity)
        {
            var tableName = typeof(T).Name;
            var field_dict = new Dictionary<string, KeyValuePair<Type, object>>();
            var field_collection = typeof(T).GetProperties();

            foreach (var field in field_collection)
            {
                field_dict[field.Name] = new KeyValuePair<Type, object>(field.PropertyType, field.GetValue(entity));
            }

            using var connection =  new SqliteConnection(connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $"delete from {tableName} where (Id = @id)";
            cmd.Parameters.AddWithValue("id", field_dict["Id"].Value);
            cmd.ExecuteNonQuery();

            transaction.Commit();
        }

        static public T Read(T entity)
        {

            var tableName = typeof(T).Name;
            var field_dict = new Dictionary<string, KeyValuePair<Type, object>>();
            var field_collection = typeof(T).GetProperties();

            foreach (var field in field_collection)
            {
                if (field.PropertyType == typeof(string) || field.PropertyType == typeof(int?) || field.PropertyType == typeof(DateTime?))
                {
                    field_dict[field.Name] = new KeyValuePair<Type, object>(field.PropertyType, field.GetValue(entity));
                }
            }

            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $"select * from {tableName} where (Id = @id)";
            cmd.Parameters.AddWithValue("id", field_dict["Id"].Value);

            using var reader = cmd.ExecuteReader();

            T new_entity = (T)typeof(T).GetConstructor(new Type[0]).Invoke(null);

            while (reader.Read())
            {
                foreach (var field in field_dict)
                {
                    var name = field.Key;
                    if (field.Value.Key == typeof(string))
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(name)))
                        {
                            var value = reader.GetString(reader.GetOrdinal(name));
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                        else
                        {
                            var value = (string?)null;
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                    }
                    else if (field.Value.Key == typeof(int?))
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(name)))
                        {
                            var value = (int)reader.GetInt64(reader.GetOrdinal(name));
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                        else
                        {
                            var value = (int?)null;
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                    }
                    else if (field.Value.Key == typeof(DateTime?))
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(name)))
                        {
                            var value = (DateTime)reader.GetDateTime(reader.GetOrdinal(name));
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                        else
                        {
                            var value = (DateTime?)null;
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid type?");
                    }
                }
            }

            transaction.Commit();
            return new_entity;
        }

        static public List<T> ReadAll()
        {
            var tableName = typeof(T).Name;
            var field_dict = new Dictionary<string, KeyValuePair<Type, object>>();
            var field_collection = typeof(T).GetProperties();

            foreach (var field in field_collection)
            {
                if (field.PropertyType == typeof(string) || field.PropertyType == typeof(int?) || field.PropertyType == typeof(DateTime?))
                {
                    field_dict[field.Name] = new KeyValuePair<Type, object>(field.PropertyType, null);
                }
            }

            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $"select * from {tableName}";

            using var reader = cmd.ExecuteReader();

            T new_entity;
            List<T> entity_list = new List<T>();

            while (reader.Read())
            {
                new_entity = (T)typeof(T).GetConstructor(new Type[0]).Invoke(null);
                
                foreach (var field in field_dict)
                {

                    var name = field.Key;
                    if (field.Value.Key == typeof(string))
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(name)))
                        {
                            var value = reader.GetString(reader.GetOrdinal(name));
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                        else
                        {
                            var value = (string?)null;
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                    }
                    else if (field.Value.Key == typeof(int?))
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(name)))
                        {
                            var value = (int)reader.GetInt64(reader.GetOrdinal(name));
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                        else
                        {
                            var value = (int?)null;
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                    }
                    else if (field.Value.Key == typeof(DateTime?))
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(name)))
                        {
                            var value = (DateTime)reader.GetDateTime(reader.GetOrdinal(name));
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                        else
                        {
                            var value = (DateTime?)null;
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid type?");
                    }
                }

                entity_list.Add(new_entity);
            }

            transaction.Commit();
            return entity_list;
        }

        static public T Update(T entity)
        {
            var tableName = typeof(T).Name;
            var field_dict = new Dictionary<string, KeyValuePair<Type, object>>();
            var update_dict = new Dictionary<string, KeyValuePair<Type, object>>();
            var field_collection = typeof(T).GetProperties();

            foreach (var field in field_collection)
            {
                if (field.PropertyType == typeof(int?) || field.PropertyType == typeof(string) || field.PropertyType == typeof(DateTime?))
                {
                    field_dict[field.Name] = new KeyValuePair<Type, object>(field.PropertyType, field.GetValue(entity));
                    update_dict[field.Name] = new KeyValuePair<Type, object>(field.PropertyType, field.GetValue(entity));
                }
            }

            var entity_id = field_dict["Id"].Value;

            if (update_dict.ContainsKey("Id"))
            {
                update_dict.Remove("Id");
            }

            if (update_dict.ContainsKey("Created"))
            {
                update_dict.Remove("Created");
            }


            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            using var cmd = connection.CreateCommand();
            StringBuilder changer = new StringBuilder();
            foreach(var field in update_dict)
            {
                if (field.Value.Key == typeof(string))
                    changer.Append($"{field.Key}='{field.Value.Value.ToString()}',");
                else if (field.Value.Key == typeof(DateTime?))
                    changer.Append($"{field.Key}='{((DateTime)field.Value.Value).ToString("yyyy-MM-ddTHH:mm:ss")}',");
                else
                    changer.Append($"{field.Key}={field.Value.Value.ToString()},");
            }

            // remove trailing ','
            changer.Length--;

            cmd.CommandText = $"update {tableName} set {changer} where Id = @Id";
            cmd.Parameters.AddWithValue("Id", entity_id);

            cmd.ExecuteNonQuery();

            using var select = connection.CreateCommand();
            select.CommandText = $"select * from {tableName} where (Id = @id)";
            select.Parameters.AddWithValue("id", entity_id);

            using var reader = select.ExecuteReader();

            T new_entity = (T)typeof(T).GetConstructor(new Type[0]).Invoke(null);

            while (reader.Read())
            {
                foreach (var field in field_dict)
                {
                    var name = field.Key;
                    if (field.Value.Key == typeof(string))
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(name)))
                        {
                            var value = reader.GetString(reader.GetOrdinal(name));
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                        else
                        {
                            var value = (string?)null;
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                    }
                    else if (field.Value.Key == typeof(int?))
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(name)))
                        {
                            var value = (int)reader.GetInt64(reader.GetOrdinal(name));
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                        else
                        {
                            var value = (int?)null;
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                    }
                    else if (field.Value.Key == typeof(DateTime?))
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(name)))
                        {
                            var value = (DateTime)reader.GetDateTime(reader.GetOrdinal(name));
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                        else
                        {
                            var value = (DateTime?)null;
                            typeof(T).GetProperty(name).SetValue(new_entity, value);
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid type?");
                    }
                }
            }

            transaction.Commit();

            return new_entity;
        }
    }
}
