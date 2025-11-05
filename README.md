## Changes to Make
1. Layered Architecture
2. Domain-Driven Design (DDD) Principles
3. Dependency Injection (DI)
4. Strategy Pattern
5. Unit and Integration Tests
6. SOLID Design Principle
7. Error Handling
8. Null Object Pattern

## Detailed changes on what and why
## 1. Layered Architecture
### What changed
- Domain Layer → Contains core business entities (Account), enums, and core business rules.
- Application Layer → Contains business logic and service orchestration (PaymentService, Validators, etc.).
- Infrastructure Layer → Handles external concerns like data access (AccountDataStore, BackupAccountDataStore).
### Why changed
- Separation of Concerns: Each layer has a single responsibility.
- Testability: You can unit test the business logic without touching the database.
- Flexibility: You can swap the data store implementation (e.g., real DB vs backup) without changing business logic.
- Maintainability: Changes in one layer don’t ripple into others.

## 2. Domain-Driven Design (DDD) Principles
### What changed
- Defines a rich domain model with entities (Account) that encapsulate business logic (Debit, SupportsScheme).
- Keeps domain rules inside the domain model, not spread across the app.
### Why changed
- Encapsulation of business rules: Domain logic like “debit must be positive” or “check payment scheme support” lives in the Account class.
- Reduces duplication: The same rules apply consistently across use cases.

## 3. Dependency Injection (DI)
### What changed
- Services and validators are injected via constructors
### Why changed
- Inversion of Control (IoC): High-level modules don’t depend on low-level implementations.
- Testability: Enables mocking dependencies during unit testing.

## 4. Strategy Pattern (Validators)
### What changed
- Each payment scheme (Bacs, Chaps, FasterPayments) has its own validator implementing a shared interface IAccountValidator.
### Why changed
- Open/Closed Principle: Add new payment schemes without modifying existing code.
- Separation of Validation Logic: Each scheme’s rules are isolated.
- Extensibility: Easy to add new validators in the future.

## 5. Interface Segregation & Abstraction
### What changed
- IAccountService, IAccountDataStore, and IPaymentService abstract behavior from implementation.
- Clients depend on interfaces, not concrete classes.
### Why changed
- Liskov Substitution: Any class implementing the interface can replace another.
- Loose Coupling: Code depends on abstractions, not implementations.
- Easier Testing: You can mock or stub interfaces in unit tests.

## 6. Error Handling
### What changed
- Account.Debit() method validates input and account balance.
### Why changed
- Defensive Programming: Prevents invalid operations early.
- Data Integrity: Ensures account balance remains consistent.
- Clear Error Feedback: Exceptions communicate exact reasons for failure.

## 7. Testability and Maintainability
### What changed
- Validators can be tested independently.
### Why changed
- Encourages Automated Testing: Business logic and validation are easy to verify.
- High Code Confidence: Easier to refactor safely.

## 8. Single Responsibility Principle (SRP)
### What changed
- Each class has one reason to change.
### Why changed
- High Cohesion: Each class does one job well.
- Low Coupling: Changes are isolated.
- Simpler Maintenance: Easier to navigate and extend.

## 9. Null Object Pattern (NullValidator)
### What changed
- When no validator is found for a scheme, PaymentService uses a NullValidator that always returns false.
### Why changed
- Avoids Null Checks Everywhere: No need for if (validator == null) logic.
- Predictable Behavior: Always returns a consistent result for unsupported schemes.

## What I would add if time permitted
1. Add Persistence (Real Database Integration)
2. Add a Dependency Injection Container & Configuration
3. Logging and Monitoring
4. Middleware Pattern (eg. Global Exceptions)
5. CQRS Pattern
6. Integration Test
7. Expose a REST API
8. Asynchronous Programming (eg. async/await methods)
9. Add Security and Validation
