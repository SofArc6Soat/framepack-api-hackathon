//using Domain.Entities;
//using Gateways.Cognito;
//using Infra.Dto;
//using Infra.Repositories;

//namespace Gateways
//{
//    public class ClienteGateway(IClienteRepository clienteRepository, IFuncionarioRepository funcionarioRepository, ICognitoGateway cognitoGateway) : IClienteGateway
//    {
//        public async Task<bool> CadastrarClienteAsync(Cliente cliente, string senha, CancellationToken cancellationToken)
//        {
//            var clienteDto = new ClienteDb
//            {
//                Id = cliente.Id,
//                Nome = cliente.Nome,
//                Email = cliente.Email,
//                Cpf = cliente.Cpf,
//                Ativo = cliente.Ativo
//            };

//            await clienteRepository.InsertAsync(clienteDto, cancellationToken);

//            return await clienteRepository.UnitOfWork.CommitAsync(cancellationToken) && await cognitoGateway.CriarUsuarioClienteAsync(cliente, senha, cancellationToken);
//        }

//        public async Task<bool> AtualizarClienteAsync(Cliente cliente, CancellationToken cancellationToken)
//        {
//            var clienteDto = new ClienteDb
//            {
//                Id = cliente.Id,
//                Nome = cliente.Nome,
//                Email = cliente.Email,
//                Cpf = cliente.Cpf,
//                Ativo = cliente.Ativo
//            };

//            await clienteRepository.UpdateAsync(clienteDto, cancellationToken);

//            return await clienteRepository.UnitOfWork.CommitAsync(cancellationToken);
//        }

//        public async Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken)
//        {
//            await clienteRepository.DeleteAsync(id, cancellationToken);

//            return await clienteRepository.UnitOfWork.CommitAsync(cancellationToken);
//        }

//        public bool VerificarClienteExistente(Guid id, string? cpf, string? email, CancellationToken cancellationToken)
//        {
//            var clienteExistente = clienteRepository.Find(e => e.Id == id || e.Cpf == cpf || e.Email == email, cancellationToken)
//                                                     .FirstOrDefault(g => g.Id == id);

//            var funcionarioExistente = funcionarioRepository.Find(e => e.Email == email, cancellationToken)
//                                                     .FirstOrDefault(g => g.Id == id);

//            return clienteExistente is not null && funcionarioExistente is null;
//        }

//        public async Task<Cliente?> VerificarClienteExistenteAsync(Guid id, CancellationToken cancellationToken)
//        {
//            var clienteExistente = await clienteRepository.FindByIdAsync(id, cancellationToken);

//            if (clienteExistente is not null)
//            {
//                var funcionarioExistente = funcionarioRepository.Find(e => e.Email == clienteExistente.Email, cancellationToken)
//                                                     .FirstOrDefault(g => g.Id == id);

//                return clienteExistente is not null && funcionarioExistente is null
//                ? new(clienteExistente.Id, clienteExistente.Nome, clienteExistente.Email, clienteExistente.Cpf, clienteExistente.Ativo)
//                : null;
//            }

//            return null;
//        }

//        public async Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken)
//        {
//            var clienteDto = await clienteRepository.ObterTodosClientesAsync(cancellationToken);

//            if (clienteDto.Any())
//            {
//                var cliente = new List<Cliente>();
//                foreach (var item in clienteDto)
//                {
//                    cliente.Add(new Cliente(item.Id, item.Nome, item.Email, item.Cpf, item.Ativo));
//                }

//                return cliente;
//            }

//            return [];
//        }
//    }
//}