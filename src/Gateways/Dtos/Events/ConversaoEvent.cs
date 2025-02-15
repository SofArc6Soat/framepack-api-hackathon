﻿using Core.Domain.Entities;

namespace Gateways.Dtos.Events
{
    public record ConversaoSolicitadaEvent : Event
    {
        public string UsuarioId { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string Status { get; set; } = string.Empty;
        public string NomeArquivo { get; set; } = string.Empty;
        public string UrlArquivoVideo { get; set; } = string.Empty;
    }

    public record DownloadEfetuadoEvent : Event
    {
        private string _urlArquivoVideo = string.Empty;

        public string UrlArquivoVideo
        {
            get => _urlArquivoVideo;
            set => _urlArquivoVideo = value ?? throw new ArgumentNullException(nameof(UrlArquivoVideo));
        }
    }
}
