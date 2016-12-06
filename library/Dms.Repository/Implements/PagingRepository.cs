using Dapper;
using Dms.Model.Enums;
using Dms.Model.Paging;
using Dms.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;


namespace Dms.Repository.Implements
{
    public class PagingRepository: IPagingRepository, IDependency
    {
        public IEnumerable<T> Find<T>(DbContext db, PagingTableModel model, out long totalCount)
        {
            totalCount = 0;
            var parameters = new DynamicParameters();
            parameters.Add("TotalCount", totalCount, DbType.Int64, direction: ParameterDirection.Output);

            string sqlCmd = string.Format(@"
                    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
                    DECLARE @PageLowerBound int
                    DECLARE @PageUpperBound int
                    SET @PageLowerBound = ({1}-1)*{2}+1 
                    SET @PageUpperBound = @PageLowerBound+{2}-1
                    SELECT @TotalCount = COUNT(1) FROM {0} {3}
                    SELECT {5} FROM (
	                    SELECT ROW_NUMBER() OVER(ORDER BY {4}) AS [row_number],* FROM {0} {3}
                    ) AS T1 WHERE T1.[row_number] BETWEEN @PageLowerBound AND @PageUpperBound",
                    model.TableName,
                    model.PageIndex,
                    model.PageSize,
                    (string.IsNullOrEmpty(model.Where) ? "" : " WHERE " + model.Where),
                    model.OrderByColumns,
                    (model.ColumnsName == "*" && model.ColumnsName.ToLower().Contains("row_number")) ? model.ColumnsName : "[row_number]," + model.ColumnsName
                );

            var list = db.Query<T>(sqlCmd, parameters, null, commandTimeout: 1000, commandType: CommandType.Text);
            totalCount = parameters.Get<long>("TotalCount");
            return list;
        }
        public IEnumerable<T> Find<T>(DbContext db, PagingJoinModel model, out long totalCount)
        {
            totalCount = 0;
            var parameters = new DynamicParameters();
            parameters.Add("TotalCount", totalCount, DbType.Int64, direction: ParameterDirection.Output);

            string sqlCmd = string.Format(@"
                    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
                    DECLARE @PageLowerBound int
                    DECLARE @PageUpperBound int
                    SET @PageLowerBound = ({1}-1)*{2}+1 
                    SET @PageUpperBound = @PageLowerBound+{2}-1
                    SELECT @TotalCount = COUNT(1) FROM {0} {3}
                    SELECT T1.*,{8} FROM (
                        SELECT T1.* FROM (
	                        SELECT ROW_NUMBER() OVER(ORDER BY {4}) AS [row_number],{5} FROM {0} {3}
                        ) AS T1
                        WHERE T1.[row_number] BETWEEN @PageLowerBound AND @PageUpperBound
                    ) AS T1
                    {9} JOIN {6} AS T2 ON {7}",
                    model.TableName,
                    model.PageIndex,
                    model.PageSize,
                    (string.IsNullOrEmpty(model.Where) ? "" : " WHERE " + model.Where),
                    model.OrderByColumns,
                    model.ColumnsName,
                    model.JoinTableName,
                    model.JoinWhere,
                    model.JoinColumnsName,
                    Enum.GetName(typeof(PagingJoinTypeEnum), model.JoinMode)
                );

            var list = db.Query<T>(sqlCmd, parameters, null, commandTimeout: 1000, commandType: CommandType.Text);
            totalCount = parameters.Get<long>("TotalCount");
            return list;
        }
        public IEnumerable<T> Find<T>(DbContext db, PagingApplyModel model, out long totalCount)
        {
            totalCount = 0;
            var parameters = new DynamicParameters();
            parameters.Add("TotalCount", totalCount, DbType.Int64, direction: ParameterDirection.Output);

            string sqlCmd = string.Format(@"
                    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
                    DECLARE @PageLowerBound int
                    DECLARE @PageUpperBound int
                    SET @PageLowerBound = ({1}-1)*{2}+1 
                    SET @PageUpperBound = @PageLowerBound+{2}-1
                    SELECT @TotalCount = COUNT(1) FROM {0} {3}
                    SELECT T1.*,{7} FROM (
	                    SELECT ROW_NUMBER() OVER(ORDER BY {4}) AS [row_number],{5} FROM {0} {3}
                    ) AS T1
                    {8} APPLY ({6}) AS T2
                    WHERE T1.[row_number] BETWEEN @PageLowerBound AND @PageUpperBound",
                    model.TableName,
                    model.PageIndex,
                    model.PageSize,
                    (string.IsNullOrEmpty(model.Where) ? "" : " WHERE " + model.Where),
                    model.OrderByColumns,
                    model.ColumnsName,
                    model.ApplyFunctionCmd,
                    model.ApplyColumnsName,
                    Enum.GetName(typeof(PagingApplyTypeEnum), model.ApplyMode)
                );

            var list = db.Query<T>(sqlCmd, parameters, null, commandTimeout: 1000, commandType: CommandType.Text);
            totalCount = parameters.Get<long>("TotalCount");
            return list;
        }
        public IEnumerable<T> Find<T>(DbContext db, FullTableModel model)
        {
            string sqlCmd = string.Format(@"
                SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
                SELECT {0} FROM {1} {2} {3}",
                model.ColumnsName,
                model.TableName,
                (string.IsNullOrEmpty(model.Where) ? "" : " WHERE " + model.Where),
                (string.IsNullOrEmpty(model.OrderByColumns) ? "" : " ORDER BY " + model.OrderByColumns));

            return db.Query<T>(sqlCmd, null, null, commandTimeout: 1000, commandType: CommandType.Text);
        }
    }
}
