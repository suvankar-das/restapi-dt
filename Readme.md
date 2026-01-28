HelpersJsonCollection has 2 files
-	movies.json (dummy movies data)
-  course.postman_collection.json (Postman collection for testing the API)
     - Import the collection into Postman to test the API endpoints.


## About the Project
- Movies.Api is a RESTful API built using ASP.NET Core that provides endpoints to manage a collection of movies. The API supports CRUD operations (Create, Read, Update, Delete) and allows users to filter movies based on various criteria.
- Movies.Application contains the business logic and service layer for the Movies API.
- Movies.Contracts defines endpoints so that later if we publish this as nuget package then enduser can consume those endpoints.


### Dapper cheatsheet

---

# 🧾 Dapper Methods – Cheatsheet

## 🔹 SELECT

```csharp
Query<T>()                  // many rows
QueryAsync<T>()

QuerySingle<T>()            // exactly 1 row
QuerySingleOrDefault<T>()   // 0 or 1 row

QueryFirst<T>()             // first row
QueryFirstOrDefault<T>()
```

---

## 🔹 INSERT / UPDATE / DELETE

```csharp
Execute()
ExecuteAsync()
```

---

## 🔹 SCALAR (single value)

```csharp
ExecuteScalar<T>()
ExecuteScalarAsync<T>()
```

---

## 🔹 MULTIPLE RESULT SETS

```csharp
QueryMultiple()
QueryMultipleAsync()
```

---
---

## 🔹 TRANSACTION USAGE

```csharp
Execute(sql, param, transaction)
Query(sql, param, transaction)
```

---

## 🔹 RULE OF THUMB

* **All writes → `Execute`**
* **Reads → `Query`**
* **Single value → `ExecuteScalar`**
* **Transaction থাকলে → সব call-এ pass করো**

---
