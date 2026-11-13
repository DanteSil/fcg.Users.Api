# UsersAPI

Cadastro, login, emissão de JWT e gestão de usuários. Execute os comandos abaixo no diretório deste serviço.

```powershell
$env:ConnectionStrings__DefaultConnection = 'Host=localhost;Database=fcg_users;Username=postgres;Password=<senha>'
$env:Jwt__SecretKey = '<chave compartilhada com o CatalogAPI, de pelo menos 32 bytes>'
$env:Seed__AdminPassword = '<senha de administrador que atende à política do domínio>'
$env:RabbitMQ__Uri = 'amqp://guest:guest@localhost:5672/%2f'
dotnet run
```

A API abre em http://localhost:5101. Use `/swagger` para consultar as rotas e `/health` para verificar que o processo está respondendo. `Jwt__Issuer` e `Jwt__Audience` têm os padrões `Fcg.Api` e `Fcg.Clients`. `Seed__AdminName` e `Seed__AdminEmail` permitem alterar a identificação do administrador inicial.

A execução direta requer .NET 8 SDK e PostgreSQL acessível pela connection string informada. Para validar as boas-vindas, mantenha RabbitMQ e NotificationsAPI rodando antes do cadastro. A URI acima exemplifica um broker local com `guest/guest`; ajuste para as credenciais do seu ambiente. O cadastro publica na exchange `usuarios`, com chave `usuario.criado`, para a fila `usuarios.notificacoes`. O e-mail é simulado nos logs de Notificações.

## Kubernetes

Os manifestos deste serviço ficam em k8s/. O script de implantação, a infraestrutura e as instruções para clonar os cinco repositórios estão em https://github.com/DanteSil/fcg.Infrastructure.
