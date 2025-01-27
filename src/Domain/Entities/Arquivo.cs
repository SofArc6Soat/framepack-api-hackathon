namespace Domain.Entities
{
    public record Arquivo
    {
        public byte[] BytesArquivo { get; private set; }
        public string NomeArquivo { get; private set; }

        public Arquivo(byte[] bytesArquivo, string nomeArquivo)
        {
            BytesArquivo = bytesArquivo;
            NomeArquivo = nomeArquivo;
        }
    }
}
