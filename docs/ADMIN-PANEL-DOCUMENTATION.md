# Thaprobid Auction Website - Admin Panel Documentation

## Table of Contents
1. [Project Overview](#project-overview)
2. [Architecture & Technology Stack](#architecture--technology-stack)
3. [Authentication & Security](#authentication--security)
4. [Admin Panel Structure](#admin-panel-structure)
5. [Component Architecture](#component-architecture)
6. [Page Layouts & Navigation](#page-layouts--navigation)
7. [Color Palette & Design System](#color-palette--design-system)
8. [API Integration](#api-integration)
9. [State Management](#state-management)
10. [Implementation Guidelines](#implementation-guidelines)

## Project Overview

The Thaprobid Auction Website Admin Panel is a comprehensive management system for controlling all aspects of the auction platform. Built to work with the .NET 8 Web API backend, it provides administrators with tools to manage users, auctions, categories, and system configurations.

### Core Admin Functionalities
- **User Management**: View, activate/deactivate user accounts
- **Category Management**: Manage Fields, Categories, and SubCategories
- **Auction Management**: Oversee auction processes and lot items
- **Content Management**: Handle platform content and configurations
- **Analytics & Reporting**: Monitor platform performance and user activities

## Architecture & Technology Stack

### Recommended Frontend Stack
```
Frontend Framework: React 18 + TypeScript
Styling: TailwindCSS + Headless UI
State Management: Zustand + React Query
Routing: React Router v6
Form Handling: React Hook Form + Zod
Charts: Recharts
Icons: Heroicons + Lucide React
HTTP Client: Axios
```

### Project Structure
```
admin-panel/
├── src/
│   ├── components/           # Reusable UI components
│   │   ├── ui/              # Base UI components
│   │   ├── forms/           # Form components
│   │   ├── charts/          # Chart components
│   │   └── layout/          # Layout components
│   ├── pages/               # Page components
│   │   ├── auth/            # Authentication pages
│   │   ├── dashboard/       # Dashboard pages
│   │   ├── users/           # User management
│   │   ├── categories/      # Category management
│   │   ├── auctions/        # Auction management
│   │   └── settings/        # System settings
│   ├── hooks/               # Custom React hooks
│   ├── services/            # API services
│   ├── stores/              # State management
│   ├── types/               # TypeScript types
│   ├── utils/               # Utility functions
│   └── constants/           # App constants
```

## Authentication & Security

### Login System
Based on the backend JWT authentication:

```typescript
// Login Flow
POST api/auth/login
{
  "email": "admin@example.com",
  "password": "password"
}

// Response
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "jwt_token_here",
    "user": {
      "id": "user_id",
      "email": "admin@example.com",
      "role": "Admin"
    }
  }
}
```

### Protected Routes
All admin routes require:
- Valid JWT token in Authorization header
- Admin role verification
- Session management with automatic token refresh

## Admin Panel Structure

### Main Navigation Structure
```
├── Dashboard
├── User Management
│   ├── All Users
│   ├── Active Users
│   ├── Inactive Users
│   └── User Details
├── Category Management
│   ├── Fields
│   ├── Categories
│   ├── SubCategories
│   └── Category Analytics
├── Auction Management
│   ├── All Auctions
│   ├── Active Auctions
│   ├── Upcoming Auctions
│   ├── Completed Auctions
│   └── Auction Analytics
├── Content Management
│   ├── Site Settings
│   ├── Email Templates
│   └── Notifications
└── Analytics & Reports
    ├── User Analytics
    ├── Auction Performance
    └── Revenue Reports
```

## Component Architecture

### 1. Layout Components

#### AdminLayout
```typescript
// components/layout/AdminLayout.tsx
interface AdminLayoutProps {
  children: React.ReactNode;
}

const AdminLayout: React.FC<AdminLayoutProps> = ({ children }) => {
  return (
    <div className="min-h-screen bg-gray-50">
      <Sidebar />
      <div className="lg:pl-64">
        <Header />
        <main className="py-6">
          <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
            {children}
          </div>
        </main>
      </div>
    </div>
  );
};
```

#### Sidebar Component
```typescript
// components/layout/Sidebar.tsx
const navigationItems = [
  { name: 'Dashboard', href: '/admin', icon: HomeIcon },
  { name: 'Users', href: '/admin/users', icon: UsersIcon },
  { name: 'Categories', href: '/admin/categories', icon: TagIcon },
  { name: 'Auctions', href: '/admin/auctions', icon: CubeIcon },
  { name: 'Analytics', href: '/admin/analytics', icon: ChartBarIcon },
];
```

### 2. UI Components

#### DataTable Component
```typescript
// components/ui/DataTable.tsx
interface DataTableProps<T> {
  data: T[];
  columns: ColumnDef<T>[];
  loading?: boolean;
  pagination?: PaginationProps;
  filters?: FilterProps;
  actions?: ActionProps<T>;
}
```

#### StatusBadge Component
```typescript
// components/ui/StatusBadge.tsx
interface StatusBadgeProps {
  status: 'active' | 'inactive' | 'pending' | 'completed';
  variant?: 'default' | 'outline';
}
```

### 3. Form Components

#### UserForm Component
```typescript
// components/forms/UserForm.tsx
interface UserFormData {
  email: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  isActive: boolean;
}
```

#### CategoryForm Component
```typescript
// components/forms/CategoryForm.tsx
interface CategoryFormData {
  name: string;
  description: string;
  fieldId: string;
  isActive: boolean;
}
```

## Page Layouts & Navigation

### 1. Dashboard Page Layout
```
┌─────────────────────────────────────────────────────────┐
│ Header (Breadcrumb + Search + Profile)                 │
├─────────────────────────────────────────────────────────┤
│ KPI Cards Row                                           │
│ ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐        │
│ │ Users   │ │Auctions │ │Revenue  │ │Activity │        │
│ └─────────┘ └─────────┘ └─────────┘ └─────────┘        │
├─────────────────────────────────────────────────────────┤
│ Charts Section                                          │
│ ┌─────────────────────┐ ┌─────────────────────┐        │
│ │ User Growth Chart   │ │ Auction Stats Chart │        │
│ └─────────────────────┘ └─────────────────────┘        │
├─────────────────────────────────────────────────────────┤
│ Recent Activities Table                                 │
└─────────────────────────────────────────────────────────┘
```

### 2. User Management Page Layout
```
┌─────────────────────────────────────────────────────────┐
│ Header (Title + Add User Button)                       │
├─────────────────────────────────────────────────────────┤
│ Filters & Search Bar                                    │
│ ┌─────────┐ ┌─────────┐ ┌─────────────────────┐        │
│ │ Status  │ │ Role    │ │ Search Users...     │        │
│ └─────────┘ └─────────┘ └─────────────────────┘        │
├─────────────────────────────────────────────────────────┤
│ Users Data Table                                        │
│ │ Avatar │ Name │ Email │ Role │ Status │ Actions │     │
│ ├────────┼──────┼───────┼──────┼────────┼─────────┤     │
│ │   👤   │ John │ john@ │Admin │ Active │ Edit    │     │
└─────────────────────────────────────────────────────────┘
```

### 3. Category Management Page Layout
```
┌─────────────────────────────────────────────────────────┐
│ Tabs Navigation (Fields | Categories | SubCategories)  │
├─────────────────────────────────────────────────────────┤
│ Content Area with CRUD Operations                       │
│ ┌─────────────────────┐ ┌─────────────────────┐        │
│ │ Category Tree View  │ │ Edit Form           │        │
│ │                     │ │                     │        │
│ │ + Field 1           │ │ Name: [_______]     │        │
│ │   - Category A      │ │ Description: [___]  │        │
│ │   - Category B      │ │ Status: [Active]    │        │
│ │ + Field 2           │ │ [Save] [Cancel]     │        │
│ └─────────────────────┘ └─────────────────────┘        │
└─────────────────────────────────────────────────────────┘
```

## Color Palette & Design System

### Primary Color Palette
```css
/* Auction Theme Colors */
:root {
  /* Primary Colors - Deep Blue */
  --primary-50: #eff6ff;
  --primary-100: #dbeafe;
  --primary-200: #bfdbfe;
  --primary-300: #93c5fd;
  --primary-400: #60a5fa;
  --primary-500: #3b82f6;  /* Main Primary */
  --primary-600: #2563eb;
  --primary-700: #1d4ed8;
  --primary-800: #1e40af;
  --primary-900: #1e3a8a;

  /* Secondary Colors - Auction Gold */
  --secondary-50: #fffbeb;
  --secondary-100: #fef3c7;
  --secondary-200: #fde68a;
  --secondary-300: #fcd34d;
  --secondary-400: #fbbf24;
  --secondary-500: #f59e0b;  /* Main Secondary */
  --secondary-600: #d97706;
  --secondary-700: #b45309;
  --secondary-800: #92400e;
  --secondary-900: #78350f;

  /* Status Colors */
  --success-500: #10b981;
  --warning-500: #f59e0b;
  --error-500: #ef4444;
  --info-500: #3b82f6;

  /* Neutral Colors */
  --gray-50: #f9fafb;
  --gray-100: #f3f4f6;
  --gray-200: #e5e7eb;
  --gray-300: #d1d5db;
  --gray-400: #9ca3af;
  --gray-500: #6b7280;
  --gray-600: #4b5563;
  --gray-700: #374151;
  --gray-800: #1f2937;
  --gray-900: #111827;
}
```

### Typography Scale
```css
/* Typography System */
.text-display-xl { font-size: 4.5rem; line-height: 1.1; }
.text-display-lg { font-size: 3.75rem; line-height: 1.1; }
.text-display-md { font-size: 3rem; line-height: 1.2; }
.text-display-sm { font-size: 2.25rem; line-height: 1.3; }

.text-heading-xl { font-size: 1.875rem; line-height: 1.3; }
.text-heading-lg { font-size: 1.5rem; line-height: 1.4; }
.text-heading-md { font-size: 1.25rem; line-height: 1.4; }
.text-heading-sm { font-size: 1.125rem; line-height: 1.5; }

.text-body-lg { font-size: 1.125rem; line-height: 1.6; }
.text-body-md { font-size: 1rem; line-height: 1.6; }
.text-body-sm { font-size: 0.875rem; line-height: 1.6; }
.text-body-xs { font-size: 0.75rem; line-height: 1.5; }
```

### Component Styling Guidelines

#### Button Styles
```css
/* Primary Button */
.btn-primary {
  @apply bg-primary-600 hover:bg-primary-700 text-white font-medium px-4 py-2 rounded-lg transition-colors;
}

/* Secondary Button */
.btn-secondary {
  @apply bg-secondary-500 hover:bg-secondary-600 text-white font-medium px-4 py-2 rounded-lg transition-colors;
}

/* Outline Button */
.btn-outline {
  @apply border border-gray-300 hover:bg-gray-50 text-gray-700 font-medium px-4 py-2 rounded-lg transition-colors;
}
```

#### Card Styles
```css
.card {
  @apply bg-white rounded-xl shadow-sm border border-gray-200 p-6;
}

.card-header {
  @apply border-b border-gray-200 pb-4 mb-4;
}
```

#### Status Indicators
```css
.status-active { @apply bg-green-100 text-green-800 px-2 py-1 rounded-full text-xs font-medium; }
.status-inactive { @apply bg-red-100 text-red-800 px-2 py-1 rounded-full text-xs font-medium; }
.status-pending { @apply bg-yellow-100 text-yellow-800 px-2 py-1 rounded-full text-xs font-medium; }
```

## API Integration

### API Service Layer
```typescript
// services/api.ts
class ApiService {
  private readonly baseURL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';
  private readonly client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: this.baseURL,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    this.setupInterceptors();
  }

  private setupInterceptors() {
    this.client.interceptors.request.use((config) => {
      const token = localStorage.getItem('token');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    });
  }
}
```

### Admin API Endpoints
```typescript
// services/adminApi.ts
export const adminApi = {
  // User Management
  users: {
    getAll: () => api.get('/admin/usemannagemet/users'),
    getById: (id: string) => api.get(`/admin/usemannagemet/users/${id}`),
    activate: (id: string) => api.post(`/admin/usemannagemet/users/${id}/activate`),
    deactivate: (id: string) => api.post(`/admin/usemannagemet/users/${id}/deactivate`),
  },

  // Category Management
  fields: {
    getAll: () => api.get('/admin/manage-fields'),
    create: (data: CreateFieldDto) => api.post('/admin/manage-fields', data),
    update: (id: string, data: UpdateFieldDto) => api.put(`/admin/manage-fields/${id}`, data),
    delete: (id: string) => api.delete(`/admin/manage-fields/${id}`),
  },

  categories: {
    getAll: () => api.get('/admin/manage-categories'),
    create: (fieldId: string, data: CreateCategoryDto) => 
      api.post(`/admin/manage-categories/${fieldId}`, data),
    update: (id: string, data: UpdateCategoryDto) => 
      api.put(`/admin/manage-categories/${id}`, data),
    delete: (id: string) => api.delete(`/admin/manage-categories/${id}`),
  },

  subCategories: {
    getAll: () => api.get('/admin/manage-subcategories'),
    create: (categoryId: string, data: CreateSubCategoryDto) => 
      api.post(`/admin/manage-subcategories/${categoryId}`, data),
    update: (id: string, data: UpdateSubCategoryDto) => 
      api.put(`/admin/manage-subcategories/${id}`, data),
    delete: (id: string) => api.delete(`/admin/manage-subcategories/${id}`),
  },
};
```

## State Management

### Zustand Store Structure
```typescript
// stores/authStore.ts
interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  login: (credentials: LoginCredentials) => Promise<void>;
  logout: () => void;
  refreshToken: () => Promise<void>;
}

// stores/usersStore.ts
interface UsersState {
  users: User[];
  selectedUser: User | null;
  loading: boolean;
  error: string | null;
  fetchUsers: () => Promise<void>;
  activateUser: (id: string) => Promise<void>;
  deactivateUser: (id: string) => Promise<void>;
}
```

### React Query Integration
```typescript
// hooks/useUsers.ts
export const useUsers = () => {
  return useQuery({
    queryKey: ['users'],
    queryFn: adminApi.users.getAll,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
};

export const useActivateUser = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: adminApi.users.activate,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['users'] });
    },
  });
};
```

## Implementation Guidelines

### 1. File Naming Conventions
```
PascalCase for components: UserManagement.tsx
camelCase for hooks: useUserData.ts
camelCase for utilities: formatDate.ts
kebab-case for pages: user-management.tsx
UPPER_CASE for constants: API_ENDPOINTS.ts
```

### 2. Component Structure Template
```typescript
// components/ComponentName.tsx
import React from 'react';
import { cn } from '@/utils/classNames';

interface ComponentNameProps {
  className?: string;
  // other props
}

export const ComponentName: React.FC<ComponentNameProps> = ({
  className,
  ...props
}) => {
  return (
    <div className={cn('default-classes', className)}>
      {/* component content */}
    </div>
  );
};

ComponentName.displayName = 'ComponentName';
```

### 3. Error Handling Strategy
```typescript
// utils/errorHandler.ts
export const handleApiError = (error: unknown) => {
  if (axios.isAxiosError(error)) {
    if (error.response?.status === 401) {
      // Handle unauthorized
      authStore.logout();
      navigate('/login');
    }
    
    return error.response?.data?.message || 'An error occurred';
  }
  
  return 'An unexpected error occurred';
};
```

### 4. Form Validation Schema
```typescript
// schemas/userSchema.ts
import { z } from 'zod';

export const userSchema = z.object({
  email: z.string().email('Invalid email address'),
  firstName: z.string().min(2, 'First name must be at least 2 characters'),
  lastName: z.string().min(2, 'Last name must be at least 2 characters'),
  phoneNumber: z.string().regex(/^\+?[\d\s-()]+$/, 'Invalid phone number'),
  isActive: z.boolean(),
});

export type UserFormData = z.infer<typeof userSchema>;
```

### 5. Responsive Design Guidelines
```css
/* Mobile First Approach */
.responsive-grid {
  @apply grid grid-cols-1 gap-4;
  @apply sm:grid-cols-2 sm:gap-6;
  @apply lg:grid-cols-3 lg:gap-8;
  @apply xl:grid-cols-4;
}

.responsive-container {
  @apply px-4 sm:px-6 lg:px-8;
  @apply max-w-7xl mx-auto;
}
```

### 6. Accessibility Guidelines
- Use semantic HTML elements
- Implement ARIA labels and roles
- Ensure keyboard navigation support
- Maintain proper color contrast ratios
- Provide screen reader support
- Implement focus management

### 7. Performance Optimization
- Implement lazy loading for routes
- Use React.memo for expensive components
- Optimize bundle size with code splitting
- Implement virtual scrolling for large lists
- Use React Query for efficient data caching
- Minimize re-renders with proper dependency arrays

## Security Considerations

### 1. Authentication & Authorization
- Store JWT tokens securely
- Implement automatic token refresh
- Handle session expiration gracefully
- Validate user permissions on protected routes

### 2. Data Validation
- Validate all user inputs
- Sanitize data before display
- Implement proper error boundaries
- Use TypeScript for type safety

### 3. API Security
- Always use HTTPS in production
- Implement request/response interceptors
- Handle rate limiting
- Validate API responses

This documentation provides a comprehensive guide for building a professional, scalable, and maintainable admin panel for the Thaprobid Auction Website. Follow these guidelines to ensure consistency and quality across the entire application.