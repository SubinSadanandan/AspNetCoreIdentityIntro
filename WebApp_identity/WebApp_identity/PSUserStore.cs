using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;


namespace WebApp_identity
{
    public class PSUserStore : IUserStore<PSUser>,IUserPasswordStore<PSUser>
    {

        public static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection("Data Source=qld_R90N9SM1\\sqlexpress;" +
                                        "database=pluralsightdemo;" +
                                        "trusted_connection=yes;");

            connection.Open();

            return connection;
        }


        public async Task<IdentityResult> CreateAsync(PSUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "insert into pluralsightusers([id]," +
                    "[username],[normalizedusername],[passwordhash])" +
                    "values(@id,@userName,@normalizedUserName,@passwordHash)",
                new
                {
                    id = user.Id,
                    userName = user.UserName,
                    normalizedUserName = user.NormalizedUserName,
                    passwordHash = user.PasswordHash
                }
                );
            }

            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(PSUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        public async Task<PSUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<PSUser>(
                    "select * from pluralsightusers where Id=@id", new { id = userId });
            }
        }

        public async Task<PSUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<PSUser>(
                    "select * from pluralsightusers where normalizedusername=@name", new { name = normalizedUserName });
            }
        }

        public Task<string> GetNormalizedUserNameAsync(PSUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(PSUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(PSUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(PSUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(PSUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetNormalizedUserNameAsync(PSUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(PSUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(PSUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(PSUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync("update pluralsightusers " +
                    "set [id]=@id,[username]=@userName," +
                    "[normalizedusername]=@normalizedusername," +
                    "[passwordhash]=@passwordhash" +
                    "where [id]=@id",
                    new
                    {
                        id=user.Id,
                        username=user.UserName,
                        normalizedusername = user.NormalizedUserName,
                        passwordhash = user.PasswordHash
                    });
            }

            return IdentityResult.Success;
        }
    }
}
