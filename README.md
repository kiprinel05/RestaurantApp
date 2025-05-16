# Restaurant Management System 🍽️

## Description
A desktop application for managing a restaurant with online ordering capabilities, developed using WPF, C# and SQL Server 2022. The application implements Data Binding concepts and follows the MVVM (Model-View-ViewModel) architecture pattern with Entity Framework for database access.

## Key Features ✨
- **Menu Management**: View the restaurant's complete menu with detailed product information
- **Advanced Search System**: Search for products by name, category, allergens and availability
- **Online Ordering**: Complete order placement and tracking for authenticated users
- **User Accounts**: Registration, authentication, and role-based access control
- **Employee Dashboard**: Comprehensive management system for restaurant staff
- **Stock Management**: Real-time tracking of product quantities and availability
- **Allergen Tracking**: Detailed allergen information for all products
- **Dynamic Pricing**: Support for special menu combinations with automatic discounts
- **Order Status Tracking**: Real-time updates on order preparation and delivery
- **Product Image Gallery**: Multiple images per product for better visualization

## Project Structure 📂

### Models
- `Allergen.cs` - Manages product allergens
- `Category.cs` - Handles product and menu categories
- `Menu.cs` - Represents special menu combinations
- `MenuProduct.cs` - Maps products to menus
- `Order.cs` - Order management
- `OrderItem.cs` - Individual items in an order
- `OrderStatus.cs` - Tracks order status (registered, preparing, delivered, etc.)
- `Product.cs` - Individual dish/product information
- `ProductImage.cs` - Handles product images
- `User.cs` - User account management

## Services Layer 🔄
The application implements a comprehensive service layer that separates business logic from UI:

### Authentication Service
- User registration with email uniqueness validation
- Secure login with password hashing
- Session management
- User role management (Customer/Employee)

### Category Service
- CRUD operations for product categories
- Category validation logic
- Relationship management with products and menus

### Product Service
- CRUD operations for restaurant products
- Product image storage and management
- Allergen association management
- Availability tracking
- Product search and filtering

### Navigation Service
- Page navigation management
- User role-based access control
- Navigation history with back functionality

## Data Context 📊
The application uses Entity Framework Core with a Code-First approach:

- `RestaurantDbContext` - Main database context that configures:
  - Entity relationships
  - Data constraints
  - Cascading behavior
  - Initial seed data
  
- `DesignTimeDbContextFactory` - Factory for design-time database context creation supporting migrations and database updates

### Views 🖼️
- Auth
  - `AuthView.xaml` - Authentication interface
- Category
  - `CategoryEditView.xaml` - Category editing
  - `CategoryListView.xaml` - Category listing
- Controls
  - `MenuCard.xaml` - Menu item card component
  - `ProductCard.xaml` - Product card component
- Employee
  - `EmployeeDashboardView.xaml` - Dashboard for employees
- Menu
  - `MenuView.xaml` - Menu display interface
- Product
  - `ProductEditView.xaml` - Product editing
  - `ProductListView.xaml` - Product listing
- `MainView.xaml` - Main application view
- `MainWindow.xaml` - Application window

### ViewModels
ViewModels connecting the data models to the views (following MVVM pattern)

## User Types 👤
The application supports three types of users:
1. **Guest Users** - Can view the menu and search for products
2. **Authenticated Customers** - Can place orders, view order history, and track active orders
3. **Employee Users** - Can manage products, categories, allergens, view all orders, and change order status

## Database Implementation 💾
- SQL Server database with Entity Framework Core ORM
- Minimum 3NF (Third Normal Form) compliant design
- Security features:
  - Password hashing using SHA-256
  - Protection against SQL Injection
- Database relationships:
  - One-to-Many: Category → Products
  - One-to-Many: Category → Menus
  - One-to-Many: User → Orders
  - One-to-Many: Order → OrderItems
  - Many-to-Many: Products ↔ Allergens
- Uses stored procedures for complex database operations
- Implements entity constraints:
  - Unique email addresses for users
  - Unique names for categories, allergens, products, and menus
  - Unique order codes
- Automatic data seeding for testing purposes

## Technical Requirements ⚙️
- .NET Framework
- Entity Framework for database access
- Stored procedures for complex queries
- MVVM architecture
- Data Binding

## Installation & Setup ⚙️

1. **Prerequisites**
   - Visual Studio 2022 or newer
   - .NET Framework 6.0+
   - SQL Server 2022
   - Entity Framework Core tools

2. **Database Setup**
   ```bash
   # Open Package Manager Console in Visual Studio
   Add-Migration InitialCreate
   Update-Database
   ```

3. **Configuration**
   - Update connection string in `DesignTimeDbContextFactory.cs` if needed
   - Default test accounts:
     - Customer: email: `customer@test.com`, password: `test123`
     - Employee: email: `employee@test.com`, password: `test123`

4. **Build & Run**
   - Build the solution in Visual Studio
   - Run the application
