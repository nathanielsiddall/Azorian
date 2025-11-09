# Endpoint BREAD Overview

This document categorizes each HTTP endpoint exposed by the Azorian API according to the BREAD (Browse, Read, Edit, Add, Delete)
actions it supports.

## PublicSchoolhouseController (`/public`)

| Route | Verb | BREAD Action | Description |
| --- | --- | --- | --- |
| `/public/index` | GET | Browse | Returns the full public overview for the current schoolhouse, including profile, media assets, instructor list, and upcoming classes. |
| `/public/instructors` | GET | Browse | Lists the public instructor profiles associated with the current schoolhouse. |
| `/public/classes` | GET | Browse | Lists published classes for the current schoolhouse. Supports the optional `futureOnly=true` query to limit the result set to upcoming classes. |
| `/public/about` | GET | Read | Returns the long-form description and public media for the current schoolhouse. |

> **Note:** The public endpoints provide read-only access. Edit/Add/Delete actions are intentionally not exposed in this controller.

## SchoolhouseController (`/schoolhouses`)

| Route | Verb | BREAD Action | Description |
| --- | --- | --- | --- |
| `/schoolhouses` | GET | Browse | Returns the list of schoolhouses with summary information suitable for administration screens. |
| `/schoolhouses/{id}` | GET | Read | Returns the full editable details for a single schoolhouse record. |
| `/schoolhouses` | POST | Add | Creates a new schoolhouse record and returns the populated resource. |
| `/schoolhouses/{id}` | PUT | Edit | Updates an existing schoolhouse record with the supplied fields. |
| `/schoolhouses/{id}` | DELETE | Delete | Removes a schoolhouse record. Related data follows the configured EF Core cascade rules. |

## IndexController (`/`)

| Route | Verb | BREAD Action | Description |
| --- | --- | --- | --- |
| `/` | GET | Read | Returns a static "Hello World" payload for service health verification. |

## Summary of Supported BREAD Actions

- **Browse**: `/public/index`, `/public/instructors`, `/public/classes`, `/schoolhouses`
- **Read**: `/public/about`, `/`, `/schoolhouses/{id}`
- **Edit**: `/schoolhouses/{id}`
- **Add**: `/schoolhouses`
- **Delete**: `/schoolhouses/{id}`

The `SchoolhouseController` introduces full management capabilities for schoolhouse records. Access to these endpoints should be protected
with appropriate authentication and authorization in production environments.
