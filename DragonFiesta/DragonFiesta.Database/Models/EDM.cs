using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using DragonFiesta.Utils.Config.Section;

namespace DragonFiesta.Database.Models
{
	public class EDM
	{
		private static DatabaseSection AccountEntitySettings { get; set; }
		private static DatabaseSection WorldEntitySettings { get; set; }

		private static AccountEntity GetAccountEntity(DatabaseSection setting) => new AccountEntity(CreateEntityString(setting));
		private static WorldEntity GetWorldEntity(DatabaseSection setting) => new WorldEntity(CreateEntityString(setting));
		
		/// <returns>New AccountEntity object</returns>
		public static AccountEntity GetAccountEntity() => new AccountEntity(CreateEntityString(AccountEntitySettings));
		/// <summary>Returns a new WorldEntity object</summary>
		/// <returns>WorldEntity</returns>
		public static WorldEntity GetWorldEntity() => new WorldEntity(CreateEntityString(WorldEntitySettings));

		// The test ensures the config is correct, before allowing static objects of the entities to be created
		public static bool TestAccountEntity(DatabaseSection setting)
		{
			try
			{
				using (var ae = GetAccountEntity(setting))
				{
					DatabaseLog.Write(DatabaseLogLevel.Startup, $"Got {ae.Accounts.Count()} Accounts from AccountEntity");
				}
				AccountEntitySettings = setting;
				return true;
			}
			catch
			{
				DatabaseLog.Write(DatabaseLogLevel.Error, "Failed to access AccountEntity, check Config files");
				return false;
			}
		}

		// The test ensures the config is correct, before allowing static objects of the entities to be created
		public static bool TestWorldEntity(DatabaseSection setting)
		{
			try
			{
				using (var we = GetWorldEntity(setting))
				{
					var i = we.DBCharacters;
					DatabaseLog.Write(DatabaseLogLevel.Startup, $"Got {we.DBCharacters.Count()} Characters from WorldEntity");
				}
				WorldEntitySettings = setting;
				return true;
			}
			catch (Exception ex)
			{
				DatabaseLog.Write(DatabaseLogLevel.Error, $"Failed to access WorldEntity: \n{ex.Message}\n{ex.StackTrace}");
				return false;
			}
		}

		public static string CreateEntityString(DatabaseSection setting)
		{
			var entityBuilder =
				new EntityConnectionStringBuilder
				{
					Provider = "System.Data.SqlClient",
					ProviderConnectionString = CreateConnectionString(setting),
					Metadata = setting.EntityMetadata
				};
			return entityBuilder.ToString();
		}

		public static string CreateConnectionString(DatabaseSection setting, bool security = false, bool multi = false)
		{
			var sqlBuilder =
				new SqlConnectionStringBuilder
				{
					DataSource = setting.SQLHost,
					InitialCatalog = setting.SQLName
				};

			// Set the properties for the data source.
			if (!(string.IsNullOrEmpty(setting.SQLUser) && string.IsNullOrEmpty(setting.SQLPassword)))
			{
				sqlBuilder.UserID = setting.SQLUser;
				sqlBuilder.Password = setting.SQLPassword;
			}
			sqlBuilder.IntegratedSecurity = security;
			sqlBuilder.MultipleActiveResultSets = multi; //allows you to have multiple datareaders at once

			// Build the SqlConnection connection string.
			return sqlBuilder.ToString();
		}
	}
}