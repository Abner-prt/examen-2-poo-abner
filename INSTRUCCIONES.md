# Documentación del Sistema de Gestión de Planillas (Examen 2 POO)

Este proyecto es una API REST desarrollada con **ASP.NET Core 10+**, **Entity Framework Core** y **SQLite**, diseñada para gestionar empleados y procesar planillas de pago de forma automática y manual.

## 🚀 Arquitectura del Proyecto

El proyecto sigue una arquitectura de capas limpia:

1.  **Entities**: Modelos de datos que representan las tablas en la base de datos (Empleado, Planilla, DetallePlanilla).
2.  **Data (DbContext)**: Configuración de la base de datos mediante EF Core.
3.  **Dtos (Data Transfer Objects)**: Objetos para el intercambio de datos entre el cliente y el servidor, incluyendo validaciones.
4.  **Services**: Capa de lógica de negocio donde se realizan los cálculos y validaciones.
5.  **Controllers**: Puntos de entrada de la API que gestionan las peticiones HTTP.

---

## 🏗️ Modelos de Datos

### 1. Empleado
Almacena la información del personal.
- **Documento (Único)**: Identificador legal (DUI, NIT, etc).
- **Salario Base**: Sueldo mensual bruto.
- **Activo**: Estado lógico del empleado. Solo los empleados activos se incluyen en la generación automática de planillas.

### 2. Planilla
Representa un periodo de pago (ej. "Marzo 2024").
- **Estados**: `Pendiente`, `Pagada`, `Anulada`.
- Las planillas en estado `Pagada` están protegidas y sus detalles no pueden ser modificados.

### 3. DetallePlanilla
Vincula a un empleado con una planilla específica y contiene el desglose del pago.
- **Cálculo del Salario Neto**:
  `Salario Neto = Salario Base + Monto Horas Extra + Bonificaciones - Deducciones`

---

## ⚙️ Funcionalidades Principales

### Gestión de Empleados
- CRUD completo.
- Validación de documento único.
- Filtrado por estado (activos/inactivos).

### Procesamiento de Planillas
- **Generación Automática**: Crea una planilla para un periodo dado e incluye automáticamente a todos los empleados activos con su salario base.
- **Cálculo de Ingresos/Egresos**: Permite actualizar horas extra, bonos y descuentos para cada empleado individualmente.
- **Control de Estados**: Permite cerrar el periodo de pago cambiando el estado a "Pagada", bloqueando ediciones futuras.

---

## 🛠️ Cómo Ejecutar el Proyecto

1.  **Restaurar dependencias**:
    ```bash
    dotnet restore
    ```
2.  **Ejecutar la aplicación**:
    ```bash
    dotnet run --project Examen2Api
    ```
3.  **Explorar la API**:
    Una vez ejecutado, abre tu navegador en:
    `https://localhost:7155/openapi/v1.json` o usa la interfaz de Swagger si está configurada (por defecto en `/swagger` si se habilitó el middleware).

---

## 📝 Flujo de Uso Recomendado

1.  **Crear Empleados**: Registra a los trabajadores con sus salarios bases.
2.  **Generar Planilla**: Usa el endpoint `POST /api/Planilla/generar-automatica` enviando el periodo (ej: "2024-03").
3.  **Ajustar Detalles**: Si un empleado tuvo horas extra o deducciones, usa `PUT /api/DetallePlanilla/{id}` para actualizar su monto. El Salario Neto se recalculará automáticamente.
4.  **Finalizar Pago**: Cambia el estado de la planilla a `Pagada` mediante `PATCH /api/Planilla/{id}/estado`.

---

**Desarrollado para el Examen 2 de Programación Orientada a Objetos.**
