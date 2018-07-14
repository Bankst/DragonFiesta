using DragonFiesta.Providers.Characters;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.Game.Characters.Data
{
    public class CharacterStyle
    {
        public HairInfo Hair { get; set; }

        public HairColorInfo HairColor { get; set; }

        public FaceInfo Face { get; set; }

        public bool RefreshFromSQL(SQLResult pRes, int i)
        {
            byte hairID = pRes.Read<byte>(i, "Hair");

            if (!CharacterLookProvider.GetHairInfoByID(hairID, out HairInfo pHair))
            {
                GameLog.Write(GameLogLevel.Warning, $"Can't find hair with ID '{hairID}' for characters");
                return false;
            }

            byte hairColorID = pRes.Read<byte>(i, "HairColor");
            if (!CharacterLookProvider.GetHairColorInfoByID(hairColorID, out HairColorInfo pHairColor))
            {
                GameLog.Write(GameLogLevel.Warning, $"Can't find hair color with ID '{hairColorID}' for chracters");
                return false;
            }

            byte FaceID = pRes.Read<byte>(i, "Face");
            if (!CharacterLookProvider.GetFaceInfoByID(FaceID, out FaceInfo pFace))
            {
                GameLog.Write(GameLogLevel.Warning, $"Can't find face with ID '{FaceID}' for a characters");
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