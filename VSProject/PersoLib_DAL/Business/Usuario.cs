﻿using System;


namespace PersoLib_DAL
{
    public static partial class Business
    {
        public class Usuario
        {
            public bool VerificaExistenciaEmail(Entity.Usuario aoUsuario)
            {
                if (new DAL.Usuario().ExisteUsuarioEmail(aoUsuario) == 1)
                {
                    return true;
                }
                return false;
            }

            public bool InserirNovoUsuario(Entity.Usuario aoUsuario, out string lsMensagemOperacao)
            {
                lsMensagemOperacao = string.Empty;
                bool lbValidado = true;

                // Confere se o nome da pessoa é muito curto
                if (!(Util.VerificarNome(aoUsuario.USR_nome)))
                {
                    lsMensagemOperacao = "Nome de tamanho invalido!";
                    lbValidado = false;
                }

                // Confere se email é valido
                if (!(Util.VerificaValidadeEmail(aoUsuario.USR_email)))
                {
                    lsMensagemOperacao = "Formato de email incorreto!";
                    lbValidado = false;
                }
                //Confere se o email informa ja foi utilizado
                if (VerificaExistenciaEmail(aoUsuario))
                {
                    lsMensagemOperacao = "Ja existe um usuario com este e-mail!";
                    lbValidado = false;
                }

                //Confere se tamanho da senha é valido
                if (!(Util.VerificaTamanhoSenha(aoUsuario.USR_senha)))
                {
                    lsMensagemOperacao = "A senha deve conter de 4 a 12 caracteres!";
                    lbValidado = false;
                }

                // Confere se os campos 'senha' e 'repete senha' são iguais
                if (!(Util.VerificaIgualdadeSenha(aoUsuario.USR_senha, aoUsuario.USR_repete_senha)))
                {
                    lsMensagemOperacao = "O campo 'repete senha' deve ser igual ao 'campo senha'!";
                    lbValidado = false;
                }

                if (lbValidado)
                {
                    if (new DAL.Usuario().InserirNovoUsuario(aoUsuario) != 1)
                    {
                        lsMensagemOperacao = "Ocorreu um erro no servidor. Tente novamente mais tarde!";
                        lbValidado = false;
                    }
                }

                return lbValidado;
            }

            public int VerificarLogin(Entity.Usuario aoUsuario, out string lsMensagemOperacao)
            {
                lsMensagemOperacao = string.Empty;
                int ID = -1;
                if (!(VerificaExistenciaEmail(aoUsuario)) || (new DAL.Usuario().LoginUsuario(aoUsuario) == -1) )
                {
                    lsMensagemOperacao = "O e-mail e/ou a senha estão incorretos!";
                    return ID;
                }
               
                ID = new DAL.Usuario().LoginUsuario(aoUsuario);
                if (!aoUsuario.USR_ativo)
                {
                    new DAL.Usuario().AtivarUsuario(aoUsuario);
                    lsMensagemOperacao = "Usuario reativado!";
                }

                return ID;
            }

            public bool AlterarUsuario(Entity.Usuario aoUsuario, out string lsMensagemOperacao)
            {
                lsMensagemOperacao = string.Empty;
                bool lbValidado = true;

                if (string.IsNullOrEmpty(aoUsuario.USR_nome) || string.IsNullOrEmpty(aoUsuario.USR_email))
                {
                    lsMensagemOperacao = "Os campos Nome e E-mail não podem ser vazios";
                    lbValidado = false;
                }

                //Confere se o nome da pessoa é muito curto
                if (!(Util.VerificarNome(aoUsuario.USR_nome)))
                {
                    lsMensagemOperacao = "Nome de tamanho invalido!";
                    lbValidado = false;
                }

                // Confere se email é valido
                if (!(Util.VerificaValidadeEmail(aoUsuario.USR_email)))
                {
                    lsMensagemOperacao = "Formato de email incorreto!";
                    lbValidado = false;
                }
                //Confere se o email informa ja foi utilizado
                if (VerificaExistenciaEmail(aoUsuario))
                {
                    lsMensagemOperacao = "Ja existe um usuario com este e-mail!";
                    lbValidado = false;
                }

                //Confere se tamanho da senha é valido
                if (!string.IsNullOrWhiteSpace(aoUsuario.USR_senha) && !(Util.VerificaTamanhoSenha(aoUsuario.USR_senha)))
                {
                    lsMensagemOperacao = "A senha deve conter de 4 a 12 caracteres!";
                    lbValidado = false;
                }

                // Confere se os campos 'senha' e 'repete senha' são iguais
                if (!(Util.VerificaIgualdadeSenha(aoUsuario.USR_senha, aoUsuario.USR_repete_senha)))
                {
                    lsMensagemOperacao = "O campo 'repete senha' deve ser igual ao 'campo senha'!";
                    lbValidado = false;
                }

                if (lbValidado)
                {
                    if (new DAL.Usuario().AtualizarDadosUsuario(aoUsuario) == -1)
                    {
                        lsMensagemOperacao = "Ocorreu algum erro no servidor!";
                        lbValidado = false;
                    }
                }

                return lbValidado;
            }

            //Carrega dados do usuario utilizando o ID do login
            public Entity.Usuario CarregarDados(Entity.Usuario aoUsuario)
            {
                Entity.Usuario loUsuario = new DAL.Usuario().ObterDadosUsuario(aoUsuario.USR_id);
                return loUsuario;
            }

            public bool DesativarUsuario(Entity.Usuario aoUsuario, out string lsMensagemOperacao)
            {
                lsMensagemOperacao = string.Empty;
                if (new DAL.Usuario().DesativarUsuario(aoUsuario) == -1)
                {
                    lsMensagemOperacao = "Ocorreu algum erro! Tente novamente!";
                    return false;
                }
                return true;
            }

        }
    }
}