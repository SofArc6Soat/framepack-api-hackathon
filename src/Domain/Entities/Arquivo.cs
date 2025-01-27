namespace Domain.Entities
{
    public record Arquivo
    {
        public byte[] BytesArquivo { get; private set; }
        public string NomeArquivo { get; private set; }

        public Arquivo(byte[] bytesArquivo, string nomeArquivo)
        {
            if (bytesArquivo == null)
                throw new ArgumentNullException(nameof(bytesArquivo));
            if (nomeArquivo == null)
                throw new ArgumentNullException(nameof(nomeArquivo));
            if (string.IsNullOrWhiteSpace(nomeArquivo))
                throw new ArgumentException("Nome do arquivo não pode ser nulo ou vazio.", nameof(nomeArquivo));

            BytesArquivo = bytesArquivo;
            NomeArquivo = nomeArquivo;
        }
    }
}
