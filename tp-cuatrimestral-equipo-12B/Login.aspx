<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="tp_cuatrimestral_equipo_12B.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0" />
    <title>Clinica Equipo 12B</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.6/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-4Q6Gf2aSP4eDXB8Miphtr37CMZZQ5oXLH2yaXMJ2w8e2ZtHTl7GptT4jmndRuHDT" crossorigin="anonymous" />
    <link href="css/styles.css" rel="stylesheet" />
    
</head>
<body class="bg-light">
    <form id="form1" runat="server" class="h-100">
     
        <div class="container h-100 d-flex justify-content-center align-items-center">
            <div class="col-12 col-md-6 col-lg-4 p-4 shadow-lg rounded bg-white">

                <h3 class="text-center mb-4">Login</h3>
                <div class="mb-3">
                    <label for="txtUsuario" class="form-label">Usuario</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtUsuario" />
                </div>
                <div class="mb-3">
                    <label for="txtPassword" class="form-label">Contraseña</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtPassword" TextMode="Password" />
                </div>
                <%-- Muestra label de error --%>
                <div class="text-center mt-3 mb-4">
                    <asp:Label ID="lblErrorLogin" runat="server" Text="Error al ingresar los datos, usuario o contraseña incorrecta" CssClass="text-danger" Visible="false" />
                </div>

                <div class="text-center d-grid gap-2">
                    <asp:Button Text="Ingresar" CssClass="btn btn-primary btn-lg" ID="btnLogin" OnClick="btnLogin_Click" runat="server" />
                    <a href="RecuperarPassword.aspx">Olvidaste la contraseña?</a>
                </div>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.6/dist/js/bootstrap.bundle.min.js" integrity="sha384-j1CDi7MgGQ12Z7Qab0qlWQ/Qqz24Gc6BM0thvEMVjHnfYGF0rmFCozFSxQBxwHKO" crossorigin="anonymous"></script>
    <script src="js/index.js"></script>
    
</body>
</html>
