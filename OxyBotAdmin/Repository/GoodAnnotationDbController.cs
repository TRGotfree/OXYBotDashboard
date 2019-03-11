using Microsoft.Extensions.Configuration;
using OxyBotAdmin.Models;
using OxyBotAdmin.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Repository
{
    public class GoodAnnotationsDbController
    {
        private readonly string connectionString;
        private readonly ILogger logger;
        private readonly int CommandTimeout;

        public GoodAnnotationsDbController(IGetConnectionString getConnectionString, ILogger _logger, IConfiguration configuration)
        {
            connectionString = getConnectionString.GetConnString();
            logger = _logger;
            CommandTimeout = configuration.GetValue<int>("CommandTimeOut");
        }

        public IEnumerable<GoodAnnotation> GetAnnotations(int beginPage, int endPage)
        {
            List<GoodAnnotation> result = new List<GoodAnnotation>(0);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.GetAnnotations, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = CommandTimeout;
                        command.Parameters.Add("@previousPage", SqlDbType.Int).Value = beginPage;
                        command.Parameters.Add("@nextPage", SqlDbType.Int).Value = endPage;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            GoodAnnotation annotation;
                            while (reader.Read())
                            {
                                annotation = new GoodAnnotation();

                                annotation.AnnotationId = reader.GetInt32(1);
                                annotation.DrugName = reader.GetString(2);
                                annotation.Producer = reader.GetString(3);
                                annotation.UsingWay = reader.GetString(4);
                                annotation.ForWhatIsUse = reader.GetString(5);
                                annotation.SpecialInstructions = reader.GetString(6);
                                annotation.ContraIndicators = reader.GetString(7);
                                annotation.SideEffects = reader.GetString(8);
                                annotation.IsImageExists = reader.GetInt32(9) > 0;
                                annotation.TotalCountOfAnnotations = reader.GetInt32(10);
                                annotation.AnnotationsWithImages = reader.GetInt32(11);
                                annotation.AnnotationsWithoutImages = reader.GetInt32(12);

                                result.Add(annotation);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw ex;
            }
            return result;
        }

        public GoodAnnotation GetAnnotation(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.GetAnnotationById, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = CommandTimeout;
                        command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            GoodAnnotation annotation = new GoodAnnotation();
                            while (reader.Read())
                            {
                                annotation.AnnotationId = reader.GetInt32(0);
                                annotation.DrugName = reader.GetString(1);
                                annotation.Producer = reader.GetString(2);
                                annotation.UsingWay = reader.GetString(3);
                                annotation.ForWhatIsUse = reader.GetString(4);
                                annotation.SpecialInstructions = reader.GetString(5);
                                annotation.ContraIndicators = reader.GetString(6);
                                annotation.SideEffects = reader.GetString(7);
                                annotation.IsImageExists = reader.GetInt32(8) > 0;
                            }
                            return annotation;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw ex;
            }
        }

        public void UpdateAnnotation(GoodAnnotation annotation)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.UpdateAnnotation, connection))
                    {
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {        
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandTimeout = CommandTimeout;
                                command.Transaction = transaction;
                                command.Parameters.Add("@id", SqlDbType.Int).Value = annotation.AnnotationId;
                                command.Parameters.Add("@usingWay", SqlDbType.NVarChar).Value = annotation.UsingWay;
                                command.Parameters.Add("@forWhatIsUse", SqlDbType.NVarChar).Value = annotation.ForWhatIsUse;
                                command.Parameters.Add("@specialInstructions", SqlDbType.NVarChar).Value = annotation.SpecialInstructions;
                                command.Parameters.Add("@contraindicators", SqlDbType.NVarChar).Value = annotation.ContraIndicators;
                                command.Parameters.Add("@sideEffects", SqlDbType.NVarChar).Value = annotation.SideEffects;

                                command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                logger.LogError(ex);
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw ex;
            }
        }

        public async Task InsertOrUpdateAnnotation(GoodAnnotation annotation)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(SqlScripts.InsertOrUpdateAnnotation, connection))
                    {
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandTimeout = CommandTimeout;
                                command.Transaction = transaction;
                                command.Parameters.Add("@annotationId", SqlDbType.Int).Value = annotation.AnnotationId;
                                command.Parameters.Add("@drugName", SqlDbType.NVarChar, 255).Value = annotation.DrugName;
                                command.Parameters.Add("@producer", SqlDbType.NVarChar, 255).Value = annotation.Producer;
                                command.Parameters.Add("@usingWay", SqlDbType.NVarChar).Value = annotation.UsingWay;
                                command.Parameters.Add("@forWhatIsUse", SqlDbType.NVarChar).Value = annotation.ForWhatIsUse;
                                command.Parameters.Add("@specialInstructions", SqlDbType.NVarChar).Value = annotation.SpecialInstructions;
                                command.Parameters.Add("@contraindicators", SqlDbType.NVarChar).Value = annotation.ContraIndicators;
                                command.Parameters.Add("@sideEffects", SqlDbType.NVarChar).Value = annotation.SideEffects;

                                await command.ExecuteNonQueryAsync();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                logger.LogError(ex);
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw ex;
            }
        }

        public async Task InsertAnnotationPhoto(int annotationId, string fileName, Stream stream)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(SqlScripts.UpdateOrInsertAnnotationImage, connection))
                    {
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandTimeout = CommandTimeout;
                                command.Transaction = transaction;
                                command.Parameters.Add("@annotationId", SqlDbType.Int).Value = annotationId;
                                command.Parameters.Add("@image", SqlDbType.VarBinary).Value = stream;
                                command.Parameters.Add("@fileName", SqlDbType.NVarChar, 300).Value = fileName;
                                
                                await command.ExecuteNonQueryAsync();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                logger.LogError(ex);
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw ex;
            }
        }

    }

}
