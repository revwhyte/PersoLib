﻿using System;

namespace PersoLib_DAL
{
    public static class Business
    {
        public class Usuario
        {
            public bool InserirNovoUsuario(Entity.Usuario aoUsuario, out string lsMensagemOperacao)
            {
                lsMensagemOperacao = string.Empty;
                bool lbValidado = true;

                // Confere comprimento da senha
                if (aoUsuario.USR_senha.Length < 4 || aoUsuario.USR_senha.Length >= 12)
                {
                    lsMensagemOperacao = "A senha deve conter de 4 a 12 caracteres!";
                    lbValidado = false;
                }

                // Confere se os campos 'senha' e 'repete senha' são iguais
                if (String.Compare(aoUsuario.USR_senha, aoUsuario.USR_repete_senha) != 0)
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
        }
    }
}