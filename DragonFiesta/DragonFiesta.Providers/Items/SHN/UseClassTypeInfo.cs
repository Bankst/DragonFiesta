using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items.SHN
{
    public class UseClassTypeInfo
    {
        public uint UseClass { get; private set; }

        public byte Fig { get; private set; }

        public byte Cfig { get; private set; }

        public byte War { get; private set; }

        public byte Gla { get; private set; }

        public byte Kni { get; private set; }

        public byte Cle { get; private set; }

        public byte Hcle { get; private set; }

        public byte Pal { get; private set; }

        public byte Hol { get; private set; }

        public byte Gua { get; private set; }

        public byte Arc { get; private set; }

        public byte Harc { get; private set; }

        public byte Sco { get; private set; }

        public byte Sha { get; private set; }

        public byte Ran { get; private set; }

        public byte Mag { get; private set; }

        public byte Wmag { get; private set; }

        public byte Enc { get; private set; }

        public byte Warl { get; private set; }

        public byte Wiz { get; private set; }

        public byte Jok { get; private set; }

        public byte Chs  { get; private set; }

        public byte Cru { get; private set; }

        public byte Cls { get; private set; }

        public byte Ass { get; private set; }

        public byte Sen { get; private set; }

        public byte Sav { get; private set; }

        public UseClassTypeInfo(SHNResult pResult, int row)
        {
            UseClass = pResult.Read<uint>(row, "UseClass");
            Fig = pResult.Read<byte>(row, "Fig");
            Cfig = pResult.Read<byte>(row, "Cfig");
            War = pResult.Read<byte>(row, "War");
            Gla = pResult.Read<byte>(row, "Gla");
            Kni = pResult.Read<byte>(row, "Kni");
            Cle = pResult.Read<byte>(row, "Cle");
            Hcle = pResult.Read<byte>(row, "Hcle");
            Pal = pResult.Read<byte>(row, "Pal");
            Hol = pResult.Read<byte>(row, "Hol");
            Gua = pResult.Read<byte>(row, "Gua");
            Arc = pResult.Read<byte>(row, "Arc");
            Harc = pResult.Read<byte>(row, "Harc");
            Sco = pResult.Read<byte>(row, "Sco");
            Sha = pResult.Read<byte>(row, "Sha");
            Ran = pResult.Read<byte>(row, "Ran");
            Mag = pResult.Read<byte>(row, "Mag");
            Wmag = pResult.Read<byte>(row, "Wmag");
            Enc = pResult.Read<byte>(row, "Enc");
            Warl = pResult.Read<byte>(row, "Warl");
            Wiz = pResult.Read<byte>(row, "Wiz");
            Jok = pResult.Read<byte>(row, "Jok");
            Chs = pResult.Read<byte>(row, "Chs");
            Cru = pResult.Read<byte>(row, "Cru");
            Cls = pResult.Read<byte>(row, "Cls");
            Ass = pResult.Read<byte>(row, "Ass");
            Sen = pResult.Read<byte>(row, "Sen");
            Sav = pResult.Read<byte>(row, "Sav");
        }
    }
}
