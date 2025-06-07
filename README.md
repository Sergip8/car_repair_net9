# Car Repair Backend

Este proyecto es un backend desarrollado en **.NET 9** para la gestión de un negocio de reparación de automóviles. Proporciona una API robusta para manejar vehículos, clientes, empleados, órdenes de trabajo, pagos, facturación y más.

## Características principales
- Gestión de vehículos, clientes, empleados y repuestos
- Administración de órdenes de trabajo y servicios
- Facturación y pagos
- Autenticación y autorización basada en JWT
- Integración con Cloudinary para gestión de imágenes
- Envío de correos electrónicos mediante SendGrid
- Documentación interactiva con Swagger/OpenAPI

## Tecnologías utilizadas
- .NET 9
- Entity Framework Core (InMemory y SQL Server)
- AutoMapper
- JWT (Json Web Token)
- Cloudinary
- SendGrid
- Swagger / Swashbuckle

## Instalación
1. **Clona el repositorio:**
   ```bash
   git clone <URL-del-repositorio>
   cd car_repair
   ```
2. **Restaura los paquetes:**
   ```bash
   dotnet restore
   ```
3. **Configura las variables de entorno o edita `appsettings.json` según tus credenciales:**
   - JWT (clave secreta, issuer, audience)
   - Cloudinary (URL de conexión)
   - SendGrid (API Key, email, nombre)
   - ConnectionStrings (cadena de conexión a base de datos)

4. **Ejecuta la aplicación:**
   ```bash
   dotnet run
   ```

5. **Accede a la documentación Swagger:**
   - [http://localhost:5167/swagger/index.html](http://localhost:5167/swagger/index.html) 

## Estructura de carpetas principal
- `Controllers/` — Controladores de la API REST (Vehículos, Usuarios, Órdenes, Pagos, etc.)
- `Models/` — Modelos de datos
- `Services/` — Lógica de negocio y servicios
- `Context/` — Contexto de base de datos
- `Helpers/`, `Middleware/`, `Extensions/`, `Config/`, `Mapper/` — Utilidades y configuración

## Configuración importante
El archivo `appsettings.json` contiene ejemplos de configuración para JWT, Cloudinary y SendGrid. **No compartas tus claves en producción.**

## Autenticación
La API utiliza JWT para autenticación y autorización. Debes incluir el token en el header `Authorization` para acceder a los endpoints protegidos.

