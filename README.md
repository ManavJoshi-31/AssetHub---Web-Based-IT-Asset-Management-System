# AssetHub – Web-Based IT Asset Management System

AssetHub is a web-based **IT Asset Management System** designed to help organizations efficiently manage and monitor their IT assets throughout their complete lifecycle.

The system provides a centralized platform for managing assets, employees, departments, asset assignments, returns, maintenance records, and warranty information. It aims to replace manual registers and spreadsheet-based asset tracking with a structured, secure, and maintainable web application.

---

## 📌 Project Overview

Organizations often manage IT assets such as laptops, desktops, monitors, printers, networking devices, and other equipment using spreadsheets or manual records.

As the number of assets increases, it becomes difficult to accurately track:

- Who currently has a particular asset
- Which assets are available
- Previous asset assignments
- Asset return history
- Maintenance activities
- Warranty information
- Asset lifecycle and current status

**AssetHub** addresses these problems by providing a centralized asset management platform with role-based access control, asset lifecycle tracking, assignment history, maintenance management, warranty monitoring, and administrative dashboards.

---

## 🎯 Objectives

The primary objectives of AssetHub are:

- Centralize organizational IT asset information.
- Maintain accurate and consistent asset records.
- Simplify asset assignment and return processes.
- Track complete asset assignment history.
- Manage asset maintenance records.
- Monitor asset warranty information.
- Track asset availability and lifecycle status.
- Prevent duplicate asset assignments.
- Provide role-based access to system functionality.
- Provide dashboards and reports for better decision-making.
- Reduce dependency on manual registers and spreadsheets.

---

## 🚨 Problem Statement

Traditional asset management approaches such as Excel spreadsheets, paper registers, and disconnected systems can result in:

- Duplicate asset allocation
- Inaccurate asset records
- Difficulty tracking asset ownership
- Loss of historical assignment information
- Poor maintenance tracking
- Missed warranty expirations
- Limited visibility into asset availability
- Increased administrative workload

AssetHub provides a centralized solution to improve the accuracy, visibility, and efficiency of IT asset management.

---

## 💡 Key Concept – Asset Lifecycle Management

AssetHub is designed around the concept of **complete asset lifecycle management**.

An asset can move through different states during its lifetime:

```text
                 ┌─────────────┐
                 │   Register  │
                 │    Asset    │
                 └──────┬──────┘
                        ↓
                 ┌─────────────┐
                 │  AVAILABLE  │
                 └──────┬──────┘
                        ↓
                 ┌─────────────┐
                 │   ASSIGNED  │
                 └──────┬──────┘
                        ↓
                 ┌─────────────┐
                 │   RETURNED  │
                 └──────┬──────┘
                        ↓
                 ┌─────────────┐
                 │  AVAILABLE  │
                 └─────────────┘

                    OR

                 ┌─────────────┐
                 │ MAINTENANCE │
                 └──────┬──────┘
                        ↓
                 ┌─────────────┐
                 │  AVAILABLE  │
                 └─────────────┘

                    OR

                 ┌─────────────┐
                 │ RETIRED /   │
                 │    LOST     │
                 └─────────────┘