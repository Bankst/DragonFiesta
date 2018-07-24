using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Database.Models
{
	public class EDM
	{
		public DBCharacter GetCharacterByID(int id)
		{
			using (var we = EntityFactory.GetWorldEntity())
			{
				var chars = we.DBCharacters as IEnumerable<DBCharacter>;
				return chars.First(x => x.ID == id);
			}
		}
	}
}
