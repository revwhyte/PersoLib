﻿using PersoLib_DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;

namespace PersoLib
{
    public partial class PaginaPrincipal : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PreenchePerfilUsuario();
                this.PreencheGridLivrosUsuario();
                this.PreencheGridEmprestimosUsuario();
            }

            if (HttpContext.Current.Session["selecao_aba"] != null)
            {
                this.ajustar_tabs();
            }
        }

        protected void AtualizarPerfil(object sender, EventArgs e)
        {
            Entity.Usuario loAlterarUsuario = new Entity.Usuario(this.txt_email.Value.ToString(), this.txt_nome.Value.ToString(), this.txt_nova_senha.Value.ToString(), this.txt_nova_senha_confirmacao.Value.ToString());
            string lsMensagemOperacao = string.Empty;
            loAlterarUsuario.USR_id = (int)Session["ID_Usuario"];
            if (!new Business.Usuario().AlterarUsuario(loAlterarUsuario, out lsMensagemOperacao))
            {
                this.div_mensagem_perfil.Visible = true;
                this.lbl_mensagem_perfil.Text = lsMensagemOperacao;
            }
            else
            {
                this.div_mensagem_perfil.Visible = true;
                this.lbl_mensagem_perfil.Text = "Seu perfil foi atualizado com sucesso!";
            }
            selecao_aba("aba3");
        }

        protected void DesativarUsuario(object sender, EventArgs e)
        {
            Entity.Usuario loUsuarioDesativado = new Entity.Usuario(string.Empty, string.Empty, string.Empty, string.Empty);
            loUsuarioDesativado.USR_id = (int)Session["ID_Usuario"];
            string lsMensagem = string.Empty;
            new Business.Usuario().DesativarUsuario(loUsuarioDesativado, out lsMensagem);
            Response.Redirect("PaginaInicial.aspx");
        }

        protected void CadastrarLivro(object sender, EventArgs e)
        {
            Entity.Usuario loUsuarioLivro = new Entity.Usuario(string.Empty, string.Empty, string.Empty, string.Empty);
            loUsuarioLivro.USR_id = (int)Session["ID_Usuario"];

            Entity.Livro loNovoLivro = new Entity.Livro(this.txt_nome_livro.Text.ToString(), loUsuarioLivro.USR_id);
            loNovoLivro.LVR_emprestado = false;
            string lsMensagemOperacao = string.Empty;

            if (!new Business.Livro().InserirNovoLivro(loNovoLivro, loUsuarioLivro, out lsMensagemOperacao))
            {
                this.div_mensagem_livro.Visible = true;
                this.lbl_mensagem_livro.Text = lsMensagemOperacao;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "abrir_popup", "<script> $('#modal_novo_livro').modal('show'); </script>", false);
            }
            else
            {
                this.alert_topo_livro.Visible = true;
                this.lbl_alert_topo_livro.Text = "Seu livro foi cadastrado com sucesso!";
                this.PreencheGridLivrosUsuario();
            }
        }

        protected void AtualizarLivro(object sender, EventArgs e)
        {
            Entity.Usuario loUsuarioAlterarLivro = new Entity.Usuario(string.Empty, string.Empty, string.Empty, string.Empty);
            loUsuarioAlterarLivro.USR_id = (int)Session["ID_Usuario"];
            Entity.Livro loAlterarLivro = new Entity.Livro(this.txt_editar_livro_nome.Value.ToString(), loUsuarioAlterarLivro.USR_id);
            if (HttpContext.Current.Session["selecao_livro"] != null)
            {
                loAlterarLivro.LVR_id = Convert.ToInt32(HttpContext.Current.Session["selecao_livro"].ToString());
                string lsMensagemOperacao = string.Empty;
                if (!new Business.Livro().AlterarLivro(loAlterarLivro, out lsMensagemOperacao))
                {
                    this.div_msg_alterar_livro.Visible = true;
                    this.lbl_msg_alterar_livro.Text = lsMensagemOperacao;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "abrir_popup", "<script> $('#edit').modal('show'); </script>", false);
                }
                else
                {
                    this.alert_topo_livro.Visible = true;
                    this.lbl_alert_topo_livro.Text = "Seu livro foi alterado com sucesso!";
                    this.PreencheGridLivrosUsuario();
                }
            }
            else
            {
                this.alert_erro_topo_livro.Visible = true;
                this.lbl_alert_erro_topo_livro.Text = "Não foi possível atualizar o livro. Tente novamente!";
            }
        }

        protected void ExcluirLivro(object sender, EventArgs e)
        {
            string lsMensagem = string.Empty;

            Entity.Livro loExcluirLivro = new Entity.Livro(string.Empty, 0);
            if (HttpContext.Current.Session["selecao_livro"] != null)
            {
                loExcluirLivro.LVR_id = Convert.ToInt32(HttpContext.Current.Session["selecao_livro"].ToString());

                if (new Business.Livro().RemoverLivro(loExcluirLivro, out lsMensagem))
                {
                    this.alert_topo_livro.Visible = true;
                    this.lbl_alert_topo_livro.Text = "Seu livro foi excluido com sucesso!";
                }
                else
                {
                    this.alert_erro_topo_livro.Visible = true;
                    this.lbl_alert_erro_topo_livro.Text = "Ocorreu um erro no servidor. Tente novamente mais tarde!";
                }
                this.PreencheGridLivrosUsuario();
            }
            else
            {
                this.alert_erro_topo_livro.Visible = true;
                this.lbl_alert_erro_topo_livro.Text = "Não foi possível excluir o livro. Tente novamente!";
            }
        }

        protected void AlterarPrazo(object sender, EventArgs e)
        {
            string lsMensagemOperacao = string.Empty;
            Entity.Emprestimo loEmprestimoAlterar = new Entity.Emprestimo(0, 0, string.Empty, string.Empty, DateTime.ParseExact(this.txt_nova_data_prazo.Value.ToString(), "dd/MM/yyyy", null));
            if (HttpContext.Current.Session["selecao_emprestimo"] != null)
            {
                loEmprestimoAlterar.EMP_id = Convert.ToInt32(HttpContext.Current.Session["selecao_emprestimo"].ToString());

                if (new Business.Emprestimo().AlterarPrazo(loEmprestimoAlterar, out lsMensagemOperacao))
                {
                    this.alert_topo_emprestimo.Visible = true;
                    this.lbl_alert_topo_emprestimo.Text = "O prazo de devolução do seu empréstimo foi alterado com sucesso!";
                    this.PreencheGridEmprestimosUsuario();
                }
                else
                {
                    this.alert_alterar_prazo.Visible = true;
                    this.lbl_alert_alterar_prazo.Text = lsMensagemOperacao;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "abrir_popup", "<script> $('#modal_alterar_prazo').modal('show'); </script>", false);
                }
            }
            else
            {
                this.alert_topo_erro_emprestimo.Visible = true;
                this.lbl_alert_topo_erro_emprestimo.Text = "Não foi possível alterar o prazo. Tente novamente!";
            }
        }

        protected void FinalizarEmprestimo(object sender, EventArgs e)
        {
            Entity.Emprestimo loEmprestimoFinalizar = new Entity.Emprestimo(0, 0, string.Empty, string.Empty, DateTime.Today);
            string lsMensagemOperacao = string.Empty;
            if (HttpContext.Current.Session["selecao_emprestimo"] != null)
            {
                loEmprestimoFinalizar.EMP_id = Convert.ToInt32(HttpContext.Current.Session["selecao_emprestimo"].ToString());

                if (new Business.Emprestimo().RealizaDevolucao(loEmprestimoFinalizar, out lsMensagemOperacao))
                {
                    this.alert_topo_emprestimo.Visible = true;
                    this.lbl_alert_topo_emprestimo.Text = "Este empréstimo foi finalizado! Agora o livro já está disponível para ser emprestado novamente!";
                    this.PreencheGridEmprestimosUsuario();
                    this.PreencheGridLivrosUsuario();
                }
                else
                {
                    this.alert_topo_erro_emprestimo.Visible = true;
                    this.lbl_alert_topo_erro_emprestimo.Text = "Ocorreu um erro no servidor. Tente novamente mais tarde!";
                }
            }
            else
            {
                this.alert_topo_erro_emprestimo.Visible = true;
                this.lbl_alert_topo_erro_emprestimo.Text = "Não foi possível finalizar o empréstimo. Tente novamente!";
            }
        }

        protected void EmprestarLivro(object sender, EventArgs e)
        {
            Entity.Usuario loUsuarioEmprestimo = new Entity.Usuario(string.Empty, string.Empty, string.Empty, string.Empty);
            loUsuarioEmprestimo.USR_id = (int)Session["ID_Usuario"];
            string lsMensagemOperacao = string.Empty;
            Entity.Livro loLivroEmprestimo = new Entity.Livro(string.Empty, loUsuarioEmprestimo.USR_id);
            if (HttpContext.Current.Session["selecao_livro"] != null)
            {
                loLivroEmprestimo.LVR_id = Convert.ToInt32(HttpContext.Current.Session["selecao_livro"].ToString());

                Entity.Emprestimo loNovoEmprestimo = new Entity.Emprestimo(loLivroEmprestimo.LVR_id, loUsuarioEmprestimo.USR_id, this.txt_email_emprestante.Value.ToString(), this.txt_nome_emprestante.Value.ToString(), Convert.ToDateTime(this.txt_nova_data.Value.ToString()));
                if (new Business.Emprestimo().InserirNovoEmprestimo(loLivroEmprestimo, loNovoEmprestimo, out lsMensagemOperacao))
                {
                    this.alert_topo_livro.Visible = true;
                    this.lbl_alert_topo_livro.Text = "Seu livro foi emprestado com sucesso. Você pode conferir na aba de Empréstimos!";
                    this.PreencheGridLivrosUsuario();
                    this.PreencheGridEmprestimosUsuario();
                }
                else
                {
                    this.div_msg_emprestimo.Visible = true;
                    this.lbl_emprestimo.Text = lsMensagemOperacao;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "abrir_popup", "<script> $('#modal_novo_emprestimo').modal('show'); </script>", false);
                }
            }
            else
            {
                this.alert_erro_topo_livro.Visible = true;
                this.lbl_alert_erro_topo_livro.Text = "Não foi possível emprestar o livro. Tente novamente!";
            }
        }

        //Procurar uma alternativa melhor
        protected void FecharPopup(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "fechar_popup_cadastro", "<script> $('#modal_novo_livro').modal('hide'); </script>", false);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "fechar_popup_cadastro2", "<script> $('#edit').modal('hide'); </script>", false);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "fechar_popup_cadastro3", "<script> $('#modal_novo_emprestimo').modal('hide'); </script>", false);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "fechar_popup_cadastro4", "<script> $('#modal_alterar_prazo').modal('hide'); </script>", false);
            this.div_mensagem_livro.Visible = false;
            this.div_msg_alterar_livro.Visible = false;
            this.div_msg_emprestimo.Visible = false;
            this.alert_topo_livro.Visible = false;
            this.alert_erro_topo_livro.Visible = false;
            this.alert_alterar_prazo.Visible = false;
            this.alert_topo_emprestimo.Visible = false;
            this.alert_topo_erro_emprestimo.Visible = false;
        }

        [WebMethod]
        public static void selecao_aba(string codigo)
        {
            HttpContext.Current.Session["selecao_aba"] = codigo;
        }

        [WebMethod]
        public static void selecao_livro(string codigo)
        {
            HttpContext.Current.Session["selecao_livro"] = codigo;
        }

        [WebMethod]
        public static void selecao_emprestimo(string codigo)
        {
            HttpContext.Current.Session["selecao_emprestimo"] = codigo;
        }
        
        /// <summary>
        /// Método responsável por preencher o grid de emprestimos do Usuário.
        /// </summary>
        protected void PreencheGridEmprestimosUsuario()
        {
            Entity.Usuario loUsuarioLogado = new Entity.Usuario(string.Empty, string.Empty, string.Empty, string.Empty);
            loUsuarioLogado.USR_id = (int)Session["ID_Usuario"];
            StringBuilder loHTMLGridEmprestimos = new StringBuilder();
            List<Entity.Emprestimo> loListaEmprestimos = new Business.Emprestimo().CarregarEmprestimos(loUsuarioLogado);
            if (loListaEmprestimos == null || loListaEmprestimos.Count == 0)
            {
                this.literal_grid_emprestimos.Text = "<h2>Você não realizou nenhum emprestimo</h2>";
            }
            else
            {
                loHTMLGridEmprestimos.Append("<table id=\"grid_emprestimos\" class=\"table table-striped table-bordered\">");
                loHTMLGridEmprestimos.Append("<thead><tr><th>Nome do Livro</th><th>Nome do Emprestante</th><th>Email do Emprestante</th><th>Data de Devolução</th><th class=\"acao\">Ações</th></tr></thead>");
                loHTMLGridEmprestimos.Append("<tbody>");
                foreach (Entity.Emprestimo loEmprestimo in loListaEmprestimos)
                {
                    loHTMLGridEmprestimos.Append("<tr><td>");
                    loHTMLGridEmprestimos.Append(loEmprestimo.EMP_nome_livro);
                    loHTMLGridEmprestimos.Append("</td><td>");
                    loHTMLGridEmprestimos.Append(loEmprestimo.EMP_nome_emprestante);
                    loHTMLGridEmprestimos.Append("</td><td>");
                    loHTMLGridEmprestimos.Append(loEmprestimo.EMP_email_emprestante);
                    loHTMLGridEmprestimos.Append("</td><td>");
                    loHTMLGridEmprestimos.Append(loEmprestimo.EMP_devolucao.ToString("dd/MM/yyyy"));
                    loHTMLGridEmprestimos.Append("</td><td><span style=\"padding-left: 14px;\"><a title=\"Alterar prazo\" class=\"btn btn-warning btn-xs\" ");
                    loHTMLGridEmprestimos.Append(" onclick=\"selecionar_emprestimo('");
                    loHTMLGridEmprestimos.Append(loEmprestimo.EMP_id);
                    loHTMLGridEmprestimos.Append("', 'PRAZO', '");
                    loHTMLGridEmprestimos.Append(loEmprestimo.EMP_devolucao.ToString("dd/MM/yyyy"));
                    loHTMLGridEmprestimos.Append("');\" ");
                    loHTMLGridEmprestimos.Append("><span class=\"glyphicon glyphicon-calendar\"></span></a>");
                    
                    loHTMLGridEmprestimos.Append("<a title=\"Finalizar empréstimo\" class=\"btn btn-success btn-xs\" ");
                    loHTMLGridEmprestimos.Append(" onclick=\"selecionar_emprestimo('");
                    loHTMLGridEmprestimos.Append(loEmprestimo.EMP_id);
                    loHTMLGridEmprestimos.Append("', 'FINALIZAR', ' ');\" ");
                    loHTMLGridEmprestimos.Append("><span class=\"glyphicon glyphicon-ok\"></span></a></span>");
                    loHTMLGridEmprestimos.Append("</td></tr></tbody></table>");
                }
                this.literal_grid_emprestimos.Text = loHTMLGridEmprestimos.ToString();
            }
        }

        /// <summary>
        /// Método responsável por preencher o grid de livros do Usuário.
        /// </summary>
        protected void PreencheGridLivrosUsuario()
        {
            Entity.Usuario loUsuarioLogado = new Entity.Usuario(string.Empty, string.Empty, string.Empty, string.Empty);
            loUsuarioLogado.USR_id = (int)Session["ID_Usuario"];
            StringBuilder loHTMLGridLivros = new StringBuilder();
            List<Entity.Livro> loListaLivros = new Business.Livro().CarregarLivros(loUsuarioLogado);
            if (loListaLivros == null || loListaLivros.Count == 0)
            {
                this.literal_grid_livros.Text = "<h2>Você não possui nenhum livro cadastrado</h2>";
            }
            else
            {
                loHTMLGridLivros.Append("<table id=\"grid_livros\" runat=\"server\" class=\"table table-striped table-bordered\">");
                loHTMLGridLivros.Append("<thead><tr><th>Nome do Livro</th><th>Emprestado</th><th class=\"acao\">Ações</th></tr></thead>");
                loHTMLGridLivros.Append("<tbody>");
                foreach (Entity.Livro loLivro in loListaLivros)
                {
                    loHTMLGridLivros.Append("<tr><td>");
                    loHTMLGridLivros.Append(loLivro.LVR_nome);
                    loHTMLGridLivros.Append("</td><td>");
                    if (loLivro.LVR_emprestado)
                    {
                        loHTMLGridLivros.Append("Sim");
                    }
                    else
                        loHTMLGridLivros.Append("Não");
                    loHTMLGridLivros.Append("</td>");
                    loHTMLGridLivros.Append("<td><a title=\"Editar este livro\" onclick=\"selecionar_livro('");
                    loHTMLGridLivros.Append(loLivro.LVR_id.ToString());
                    loHTMLGridLivros.Append("', 'EDITAR', '");
                    loHTMLGridLivros.Append(loLivro.LVR_nome);
                    loHTMLGridLivros.Append("');\"");
                    loHTMLGridLivros.Append("class=\"btn btn-primary btn-xs\">");
                    loHTMLGridLivros.Append("<span class=\"glyphicon glyphicon-pencil\"></span></a>");
                    if (loLivro.LVR_emprestado)
                        loHTMLGridLivros.Append("<a title=\"Excluir este livro\" disabled onclick=\"selecionar_livro('");
                    else
                        loHTMLGridLivros.Append("<a title=\"Excluir este livro\" onclick=\"selecionar_livro('");
                    loHTMLGridLivros.Append(loLivro.LVR_id.ToString());
                    loHTMLGridLivros.Append("', 'EXCLUIR', ' ');\"");
                    loHTMLGridLivros.Append("class=\"btn btn-danger btn-xs\">");

                    loHTMLGridLivros.Append("<span class=\"glyphicon glyphicon-trash\"></span></a>");
                    if(loLivro.LVR_emprestado)
                        loHTMLGridLivros.Append("<a title=\"Empreste este livro\" disabled onclick=\"selecionar_livro('");
                    else
                        loHTMLGridLivros.Append("<a title=\"Empreste este livro\" onclick=\"selecionar_livro('");
                    
                    loHTMLGridLivros.Append(loLivro.LVR_id.ToString());
                    loHTMLGridLivros.Append("', 'EMPRESTAR', '");
                    loHTMLGridLivros.Append(loLivro.LVR_nome);
                    loHTMLGridLivros.Append("');\"");
                    loHTMLGridLivros.Append("class=\"btn btn-warning btn-xs\">");
                    loHTMLGridLivros.Append("<span class=\"glyphicon glyphicon-new-window\"></span></a>");
                    loHTMLGridLivros.Append("</td></tr>");
                }
                loHTMLGridLivros.Append("</tbody>");
                loHTMLGridLivros.Append("</table>");
                this.literal_grid_livros.Text = loHTMLGridLivros.ToString();
            }
        }
        /// <summary>
        /// Método responsável por preencher os campos do Perfil do Usuário.
        /// </summary>
        protected void PreenchePerfilUsuario()
        {
            Entity.Usuario loUsuarioLogado = new Entity.Usuario(string.Empty, string.Empty, string.Empty, string.Empty);
            loUsuarioLogado.USR_id = (int)Session["ID_Usuario"];
            loUsuarioLogado = new Business.Usuario().CarregarDados(loUsuarioLogado);
            this.txt_nome.Value = loUsuarioLogado.USR_nome;
            this.txt_email.Value = loUsuarioLogado.USR_email;
        }

        /// <summary>
        /// Método responsável por ajustar as tabs toda vem que um pushback é gerado.
        /// </summary>
        private void ajustar_tabs()
        {
            String aba = (string)HttpContext.Current.Session["selecao_aba"];
            if (aba == "aba1")
            {
                this.div_aba2.Attributes["class"] = this.div_aba2.Attributes["class"].Replace("in", "");
                this.div_aba2.Attributes["class"] = this.div_aba2.Attributes["class"].Replace("active", "");
                this.div_aba3.Attributes["class"] = this.div_aba2.Attributes["class"].Replace("in", "");
                this.div_aba3.Attributes["class"] = this.div_aba2.Attributes["class"].Replace("active", "");
                this.div_aba2.Attributes.Add("class", "tab-pane fade");
                this.div_aba3.Attributes.Add("class", "tab-pane fade");
                this.li_aba1.Attributes.Add("class", "active");
                this.li_aba2.Attributes.Remove("class");
                this.li_aba3.Attributes.Remove("class");
                this.div_aba1.Attributes.Add("class", "in");
                this.div_aba1.Attributes.Add("class", "active");
            }
            else if (aba == "aba2")
            {
                this.div_aba1.Attributes["class"] = this.div_aba2.Attributes["class"].Replace("in", "");
                this.div_aba1.Attributes["class"] = this.div_aba2.Attributes["class"].Replace("active", "");
                this.div_aba3.Attributes["class"] = this.div_aba2.Attributes["class"].Replace("in", "");
                this.div_aba3.Attributes["class"] = this.div_aba2.Attributes["class"].Replace("active", "");
                this.li_aba2.Attributes.Add("class", "active");
                this.li_aba1.Attributes.Remove("class");
                this.li_aba3.Attributes.Remove("class");
                this.div_aba3.Attributes.Add("class", "tab-pane fade");
                this.div_aba1.Attributes.Add("class", "tab-pane fade");
                this.div_aba2.Attributes.Add("class", "in");
                this.div_aba2.Attributes.Add("class", "active");
            }
            else
            {
                this.div_aba1.Attributes["class"] = this.div_aba2.Attributes["class"].Replace("in", "");
                this.div_aba1.Attributes["class"] = this.div_aba2.Attributes["class"].Replace("active", "");
                this.div_aba2.Attributes["class"] = this.div_aba2.Attributes["class"].Replace("in", "");
                this.div_aba2.Attributes["class"] = this.div_aba2.Attributes["class"].Replace("active", "");
                this.li_aba3.Attributes.Add("class", "active");
                this.li_aba2.Attributes.Remove("class");
                this.li_aba1.Attributes.Remove("class");
                this.div_aba2.Attributes.Add("class", "tab-pane fade");
                this.div_aba1.Attributes.Add("class", "tab-pane fade");
                this.div_aba3.Attributes.Add("class", "in");
                this.div_aba3.Attributes.Add("class", "active");
            }
        }
    }
}


