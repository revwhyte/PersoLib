﻿using System.Collections.Generic;
namespace PersoLib_DAL
{
    public static partial class Business
    {
        public class Emprestimo
        {
            public bool VerificaData(Entity.Emprestimo aoEmprestimo)
            {
                if (new DAL.Emprestimo().Datavalida(aoEmprestimo) == 1)
                {
                    return true;
                }
                return false;
            }
            
            public bool InserirNovoEmprestimo(Entity.Emprestimo aoEmprestimo, out string lsMensagemOperacao)
            {
                lsMensagemOperacao = string.Empty;
                bool lbValidado = true;
                
                //Confere se o nome da pessoa é muito curto
                if (!(Util.VerificarNome(aoEmprestimo.EMP_nome_emprestante)))
                {
                    lsMensagemOperacao = "Nome de tamanho invalido!";
                    lbValidado = false;
                }
                
                // Confere se email é valido
                if (!(Util.VerificaValidadeEmail(aoEmprestimo.EMP_email_emprestante)))
                {
                    lsMensagemOperacao = "Formato de email incorreto!";
                    lbValidado = false;
                }
                //Confere se a data é valida
                if (Util.VerificaData(aoEmprestimo))
                {
                    lsMensagemOperacao = "A data não pode ser anterior ao dia atual";
                    lbValidado = false;
                }
                
                if (lbValidado)
                {
                    if (new DAL.Emprestimo().InserirNovoEmprestimo(aoEmprestimo) != 1)
                    {
                        lsMensagemOperacao = "Ocorreu um erro no servidor. Tente novamente mais tarde!";
                        lbValidado = false;
                    }
                }

                return lbValidado;
            }

            
            //Altera o prazo de entrega do livro emprestado
            public bool AlterarPrazo(Entity.Emprestimo aoEmprestimo, out string lsMensagemOperacao)
            {
                lsMensagemOperacao = string.Empty;
                bool lbValidado = true;

                //Confere se a data é valida
                if (Util.VerificaData(aoEmprestimo))
                {
                    lsMensagemOperacao = "A data não pode ser anterior ao dia atual";
                    lbValidado = false;
                }

                if (lbValidado)
                {
                    if (new DAL.Emprestimo().AtualizarDataEmprestimo(aoEmprestimo) == -1)
                    {
                        lsMensagemOperacao = "Ocorreu algum erro no servidor!";
                        lbValidado = false;
                    }
                }

                return lbValidado;
            }

            //Carrega dados do emprestimo utilizando o ID do Usuario
            public List<Entity.Emprestimo> CarregarEmprestimo(Entity.Usuario aoUsuario, out string lsMensagemOperacao)
            {
                lsMensagemOperacao = string.Empty;
              
                List<Entity.Emprestimo> loEmprestimo = new DAL.Emprestimo().ObterDadosEmprestimos(aoUsuario.USR_id);
                if (loEmprestimo.Count == 0)
                {
                    lsMensagemOperacao = "Voce não realizou empréstimos.";
                }
                else
                {
                    if(loEmprestimo == null)
                        lsMensagemOperacao = "Ocorreu algum erro! Tente novamente!";
                }
                return loEmprestimo;
            }
            
            //Finaliza um emprestimo usando id do usuario e do emprestimo
            public bool RealizaDevolucao(Entity.Usuario aoUsuario, Entity.Emprestimo aoEmprestimo,out string lsMensagemOperacao)
            {
                lsMensagemOperacao = string.Empty;
                if (new DAL.Emprestimo().ConcluiEMprestimo(aoUsuario, aoEmprestimo) == -1)
                {
                    lsMensagemOperacao = "Ocorreu algum erro! Tente novamente!";
                    return false;
                }
                return true;
            }

        }
    }
}