# UsersAPI

Cadastro, login, emissão de JWT e gestão de usuários. Execute os comandos abaixo no diretório deste serviço.

```powershell
$env:ConnectionStrings__DefaultConnection = 'Host=localhost;Database=fcg_users;Username=postgres;Password=<senha>'
$env:Jwt__SecretKey = '<chave compartilhada com o CatalogAPI, de pelo menos 32 bytes>'
$env:Seed__AdminPassword = '<senha de administrador que atende à política do domínio>'
dotnet run
```

A API abre em http://localhost:5101. Use `/swagger` para consultar as rotas e `/health` para verificar que o processo está respondendo. `Jwt__Issuer` e `Jwt__Audience` têm os padrões `Fcg.Api` e `Fcg.Clients`. `Seed__AdminName` e `Seed__AdminEmail` permitem alterar a identificação do administrador inicial.

## Kubernetes

Os manifestos deste serviço ficam em k8s/. O script de implantação, a infraestrutura e as instruções para clonar os cinco repositórios estão em https://github.com/DanteSil/fcg.Infrastructure.
