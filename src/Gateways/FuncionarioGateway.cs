//using Domain.Entities;
//using Gateways.Cognito;
//using Infra.Dto;
//using Infra.Repositories;

//namespace Gateways
//{
//    public class FuncionarioGateway(IFuncionarioRepository funcionarioRepository, IClienteRepository clienteRepository, ICognitoGateway cognitoGateway) : IFuncionarioGateway
//    {
//        public async Task<bool> CadastrarFuncionarioAsync(Funcionario funcionario, string senha, CancellationToken cancellationToken)
//        {
//            var funcionarioDto = new FuncionarioDb
//            {
//                Id = funcionario.Id,
//                Nome = funcionario.Nome,
//                Email = funcionario.Email,
//                Ativo = funcionario.Ativo
//            };

//            await funcionarioRepository.InsertAsync(funcionarioDto, cancellationToken);

//            return await funcionarioRepository.UnitOfWork.CommitAsync(cancellationToken) && await cognitoGateway.CriarUsuarioFuncionarioAsync(funcionario, senha, cancellationToken);
//        }

//        public bool VerificarFuncionarioExistente(Guid id, string? email, CancellationToken cancellationToken)
//        {
//            var funcionarioExistente = funcionarioRepository.Find(e => e.Id == id || e.Email == email, cancellationToken)
//                                                     .FirstOrDefault();

//            if (funcionarioExistente is not null)
//            {
//                var clienteExistente = clienteRepository.Find(e => e.Email == funcionarioExistente.Email, cancellationToken)
//                                                    .FirstOrDefault();

//                return clienteExistente is null;
//            }

//            return false;
//        }
//    }
//}