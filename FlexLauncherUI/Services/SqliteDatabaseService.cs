using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using FlexLauncherUI.Data;
using Microsoft.Data.Sqlite;

namespace FlexLauncherUI.Services;

public class SqliteDatabaseService : IDatabaseService
{
    private readonly string path;
    
    public SqliteDatabaseService(string path)
    {
        this.path = path;
    }
    
    public async IAsyncEnumerable<Profile> FetchProfilesAsync()
    {
        await using var connection = await OpenConnectionAsync();
        
        // Query the database for all profiles
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM profiles";

        // Read the profiles from the database
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var id = reader.GetString("id");
            var name = reader.GetString("name");
            var lastUpdatedMillis = reader.GetInt64("last_updated");
            var dateCreatedMillis = reader.GetInt64("date_created");
            var playTimeMillis = reader.GetInt64("play_time");
            
            // Convert unix millis to their respective types
            var lastUpdatedOffset = DateTimeOffset.FromUnixTimeMilliseconds(lastUpdatedMillis);
            var dateCreatedOffset = DateTimeOffset.FromUnixTimeMilliseconds(dateCreatedMillis);
            var lastUpdated = lastUpdatedOffset.DateTime;
            var dateCreated = dateCreatedOffset.DateTime;
            var playTime = TimeSpan.FromMilliseconds(playTimeMillis);

            // Yield the profile
            yield return new Profile(id, name, lastUpdated, dateCreated, playTime);
        }
    }

    public async IAsyncEnumerable<Account> FetchAccountsAsync()
    {
        await using var connection = await OpenConnectionAsync();
        
        // Query the database for all accounts
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM accounts";
        
        // Read the accounts from the database
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var id = reader.GetString("id");
            var uuid = reader.GetString("uuid");
            var username = reader.GetString("username");
            var token = reader.GetString("token");
            var refreshToken = reader.GetString("refresh_token");
            
            // Yield the account
            yield return new Account(id, uuid, username, token, refreshToken);
        }
    }

    private async Task<DbConnection> OpenConnectionAsync()
    {
        var connection = new SqliteConnection($"Data Source={path}");
        await connection.OpenAsync();
        return connection;
    }
}