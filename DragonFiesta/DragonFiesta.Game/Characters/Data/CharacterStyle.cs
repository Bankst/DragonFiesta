#region

using DragonFiesta.Database.Models;
using DragonFiesta.Database.SQL;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Utils.Logging;

#endregion

namespace DragonFiesta.Game.Characters.Data
{
    public class CharacterStyle
    {
	    public HairInfo Hair { get; set; }

        public HairColorInfo HairColor { get; set; }

        public FaceInfo Face { get; set; }

	    public bool RefreshFromEntity(DBCharacter dbCharacter)
	    {
		    var hairID = dbCharacter.Hair;
		    if (!CharacterLookProvider.GetHairInfoByID(hairID, out var pHair))
		    {
			    GameLog.Write(GameLogLevel.Warning, $"Can't find hair with ID '{hairID}' for characters");
			    return false;
		    }

			var hairColorID = dbCharacter.HairColor;
		    if (!CharacterLookProvider.GetHairColorInfoByID(hairColorID, out var pHairColor))
		    {
			    GameLog.Write(GameLogLevel.Warning, $"Can't find hair color with ID '{hairColorID}' for chracters");
			    return false;
		    }

			var faceID = dbCharacter.Face;
		    if (!CharacterLookProvider.GetFaceInfoByID(faceID, out var pFace))
		    {
			    GameLog.Write(GameLogLevel.Warning, $"Can't find face with ID '{faceID}' for a characters");
			    return false;
		    }

		    Hair = pHair;
		    HairColor = pHairColor;
		    Face = pFace;

		    return true;
		}

        public bool RefreshFromSQL(SQLResult pRes, int i)
        {
            var hairID = pRes.Read<byte>(i, "Hair");

            if (!CharacterLookProvider.GetHairInfoByID(hairID, out var pHair))
            {
                GameLog.Write(GameLogLevel.Warning, $"Can't find hair with ID '{hairID}' for characters");
                return false;
            }

            var hairColorID = pRes.Read<byte>(i, "HairColor");
            if (!CharacterLookProvider.GetHairColorInfoByID(hairColorID, out var pHairColor))
            {
                GameLog.Write(GameLogLevel.Warning, $"Can't find hair color with ID '{hairColorID}' for chracters");
                return false;
            }

            var faceID = pRes.Read<byte>(i, "Face");
            if (!CharacterLookProvider.GetFaceInfoByID(faceID, out var pFace))
            {
                GameLog.Write(GameLogLevel.Warning, $"Can't find face with ID '{faceID}' for a characters");
                return false;
            }
            Hair = pHair;
            HairColor = pHairColor;
            Face = pFace;

            return true;
        }

        ~CharacterStyle()
        {
            Hair = null;
            HairColor = null;
            HairColor = null;
            Face = null;
        }
    }
}