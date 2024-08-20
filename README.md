# Gym Management API

## Overview

This API is designed to manage a gym's operations, including user management, workout plans, nutrition plans, classes, attendance, and more. The project follows the Clean Architecture pattern and includes various features such as refresh tokens, soft deletes, and robust error handling.

## Features

- **User Management**: Handles user registration, authentication, and profile management.
- **Workout Plans**: Create and manage workout plans tailored to user needs.
- **Nutrition Plans**: Create and manage nutrition plans.
- **Class Management**: Schedule and manage gym classes.
- **Attendance Tracking**: Track attendance for classes and gym visits.
- **Feedback System**: Collect and manage user feedback.
- **BMI Records**: Track user BMI over time.
- **Meal Planning**: Organize and manage meals and their categories.
- **Notifications**: Send notifications to users.
- **Soft Delete**: Implemented across entities to ensure data isn't permanently removed but rather flagged as deleted.
- **Refresh Tokens**: Used for secure user sessions and to maintain authentication without requiring frequent logins.

## Project Structure

### Controllers
- **AccountController**: Manages user accounts and authentication.
- **AttendanceController**: Handles user attendance.
- **AuthController**: Manages authorization and token generation.
- **BMIRecordController**: Manages BMI records.
- **ClassController**: Handles gym classes.
- **ExerciseCategoryController**: Manages exercise categories.
- **ExerciseController**: Manages exercises within workout plans.
- **FeedbackController**: Handles user feedback.
- **MealController**: Manages meals within nutrition plans.
- **MealsCategoriesController**: Handles meal categories.
- **MembershipController**: Manages memberships and subscriptions.
- **NotificationController**: Sends notifications to users.
- **NutritionPlanController**: Manages nutrition plans.
- **WorkoutPlanController**: Manages workout plans.

### Database Schema
The database schema is structured around the key entities of the gym management system:

- **AspNetUsers**: Central user table.
- **WorkoutPlans**: Holds workout plans with exercises.
- **NutritionPlans**: Manages user nutrition plans.
- **Classes**: Represents gym classes and schedules.
- **Attendances**: Tracks user attendance in classes.
- **Memberships**: Manages gym memberships and subscriptions.
- **Notifications**: Sends notifications to users.
- **BMIRecords**: Stores BMI data for users.
- **Exercises**: Contains exercises for workout plans.
- **ExerciseCategories**: Categorizes exercises.
- **Meals**: Stores meal data for nutrition plans.
- **MealsCategories**: Categorizes meals.
- **Feedbacks**: Manages user feedback.
- **RefreshTokens**: Manages refresh tokens for secure authentication.

## Getting Started

### Prerequisites
- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

### Installation

1. **Clone the repository**:
    ```bash
    git clone https://github.com/your-repo/gym-management-api.git
    ```

2. **Navigate to the project directory**:
    ```bash
    cd gym-management-api
    ```

3. **Restore the dependencies**:
    ```bash
    dotnet restore
    ```

4. **Update the database**:
    ```bash
    dotnet ef database update
    ```

5. **Run the application**:
    ```bash
    dotnet run
    ```

### API Endpoints

| Endpoint                        | Method | Description                                  |
|---------------------------------|--------|----------------------------------------------|
| `/api/account/register`         | POST   | Register a new user                          |
| `/api/auth/login`               | POST   | Login and generate tokens                    |
| `/api/workoutplans`             | GET    | Get all workout plans                        |
| `/api/nutritionplans`           | GET    | Get all nutrition plans                      |
| `/api/classes`                  | GET    | Get all gym classes                          |
| `/api/attendance`               | POST   | Record attendance                            |
| `/api/feedback`                 | POST   | Submit user feedback                         |

