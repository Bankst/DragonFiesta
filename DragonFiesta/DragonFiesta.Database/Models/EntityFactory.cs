using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using DragonFiesta.Utils.Config.Section;

namespace DragonFiesta.Database.Models
{
	public class EntityFactory
	{
		private static DatabaseSection AccountEntitySettings { get; set; }
		private static DatabaseSection WorldEntitySettings { get; set; }

		private static AccountEntity GetAccountEntity(DatabaseSection setting) => new AccountEntity(CreateEntityString(setting));
		
		private static WorldEntity GetWorldEntity(DatabaseSection setting) => new WorldEntity(CreateEntityString(setting));

		public static bool TestAccountEntity(DatabaseSection setting)
		{
			try
			{
				DatabaseLog.Write(DatabaseLogLevel.Debug, $"Got {GetAccountEntity(setting).Accounts.Count()} Accounts from AccountEntity");
				return true;
			}
			catch
			{
				DatabaseLog.Write(DatabaseLogLevel.Error, "Failed to access AccountEntity");
				return false;
			}
		}

		public static AccountEntity GetAccountEntity()
		{
			return AccountEntitySettings == null ? null : GetAccountEntity(AccountEntitySettings);
		}

		public static WorldEntity GetWorldEntity()
		{
			return WorldEntitySettings == null ? null : GetWorldEntity(WorldEntitySettings);
		}

		// The test ensures the config is correct, before allowing static objects of the entities to be created
		public static bool TestWorldEntity(DatabaseSection setting)
		{
			try
			{
				using (var we = GetWorldEntity(setting))
				{
					DatabaseLog.Write(DatabaseLogLevel.Startup, $"Got {we.DBCharacters.Count()} Characters from WorldEntity");
				}
				WorldEntitySettings = setting;
				return true;
			}
			catch
			{
				DatabaseLog.Write(DatabaseLogLevel.Error, "Failed to access WorldEntity, check Config files");
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