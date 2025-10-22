# 🌾 AgroAPI

**AgroAPI** es una aplicación web desarrollada con **.NET 8**, **Entity Framework Core** y una **arquitectura Clean (Clean Architecture)**.  
Su propósito es ofrecer una API modular y escalable para la gestión de usuarios, productos y órdenes de venta dentro de un entorno agroindustrial.

---

## 🏗️ Arquitectura

El proyecto sigue los principios de **Clean Architecture**, dividiendo el código en capas bien definidas:

src/
├── Application/ # Lógica de negocio y casos de uso
│ ├── Interfaces/ # Definición de contratos (servicios y repositorios)
│ └── Services/ # Implementación de los casos de uso
│
├── Domain/ # Entidades del dominio y repositorios base
│ └── Interfaces/ # Contratos que deben implementar las capas inferiores
│
├── Infrastructure/ # Implementación de persistencia y servicios externos
│ ├── Repositories/ # Acceso a datos mediante Entity Framework Core
│ └── Services/ # Autenticación, JWT, y utilidades
│
└── web/ # Capa de presentación (API controllers)
└── Controllers/ # Endpoints de la API

---

## ⚙️ Tecnologías utilizadas

- **.NET 8 Web API**
- **Entity Framework Core**
- **JWT Authentication**
- **SQL Server / SQLite (según configuración)**
- **Clean Architecture**
- **Dependency Injection**
- **Data Annotations y DTOs**

---

## 🔐 Autenticación

La API utiliza **JWT (JSON Web Token)** para la autenticación y autorización de usuarios.  
El servicio `AuthenticationService` se encarga de:

- Validar credenciales del usuario.
- Emitir el token JWT.
- Definir los roles (`Admin`, `Seller`, `Client`).

---

## 📦 Capas principales

### Application
Contiene la lógica del negocio y los **casos de uso**.  
Ejemplo: `UserService`, `ProductService`, `SaleOrderService`.

### Domain
Define las **entidades centrales** (`User`, `Product`, `SaleOrder`, etc.) y las interfaces que deben implementarse en `Infrastructure`.

### Infrastructure
Implementa las interfaces del dominio:
- `UserRepository` → Persistencia de usuarios.
- `ProductRepository` → Gestión de productos.
- `SaleOrderRepository` → Manejo de órdenes de venta.

Incluye además el `AuthenticationService` para la emisión y validación de tokens JWT.

### Web
Capa que expone la API mediante **Controllers**:
- `UserController`
- `ProductController`
- `SaleOrderController`
- `AuthenticationController`

---

## 🚀 Ejecución del proyecto

### 1️⃣ Clonar el repositorio
```bash
git clone https://github.com/Alvarojohansen/AgroAPI.git
cd AgroAPI/src/web

### 2️⃣ Configurar la base de datos

Editar el archivo appsettings.json con tu cadena de conexión.

3️⃣ Aplicar migraciones

dotnet ef database update

4️⃣ Ejecutar la API

dotnet run

| Método | Endpoint             | Descripción                                  |
| :----- | :------------------- | :------------------------------------------- |
| `POST` | `/api/auth/login`    | Autentica un usuario y devuelve un token JWT |
| `POST` | `/api/user/postUser` | Registra un nuevo usuario                    |
| `GET`  | `/api/product`       | Obtiene la lista de productos                |
| `POST` | `/api/saleorder`     | Crea una nueva orden de venta                |

