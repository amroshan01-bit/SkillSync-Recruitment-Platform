import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';
import { AdminDashboardComponent } from './features/admin/admin-dashboard/admin-dashboard.component';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { UnauthorizedComponent } from './features/auth/unauthorized/unauthorized.component';
import { EmployerProfileComponent } from './features/employer/employer-profile/employer-profile.component';
import { VacancyManagementComponent } from './features/employer/vacancy-management/vacancy-management.component';
import { JobSeekerProfileComponent } from './features/seeker/job-seeker-profile/job-seeker-profile.component';
import { JobMatchesComponent } from './features/seeker/job-matches/job-matches.component';
import { MyApplicationsComponent } from './features/seeker/my-applications/my-applications.component';
import { ManageSkillsComponent } from './features/seeker/manage-skills/manage-skills.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    title: 'Login | SkillSync',
  },
  {
    path: 'register',
    component: RegisterComponent,
    title: 'Register | SkillSync',
  },
  {
    path: 'unauthorized',
    component: UnauthorizedComponent,
    title: 'Access Denied | SkillSync',
  },
  {
    path: 'admin/dashboard',
    component: AdminDashboardComponent,
    canActivate: [
      authGuard,
      roleGuard,
    ],
    data: {
      roles: ['Admin'],
    },
    title: 'Admin Dashboard | SkillSync',
  },
  {
    path: 'seeker/profile',
    component: JobSeekerProfileComponent,
    canActivate: [
      authGuard,
      roleGuard,
    ],
    data: {
      roles: ['JobSeeker'],
    },
  },
   {
    path: 'seeker/matches',
    component: JobMatchesComponent,
    canActivate: [
      authGuard,
      roleGuard,
    ],
    data: {
      roles: ['JobSeeker'],
    },
  },
  {
    path: 'seeker/applications',
    component: MyApplicationsComponent,
    canActivate: [
      authGuard,
      roleGuard,
    ],
    data: {
      roles: ['JobSeeker'],
    },
  },
  {
    path: 'seeker/skills',
    component: ManageSkillsComponent,
    canActivate: [
      authGuard,
      roleGuard,
    ],
    data: {
      roles: ['JobSeeker'],
    },
  },  {
    path: 'employer/profile',
    component: EmployerProfileComponent,
    canActivate: [
      authGuard,
      roleGuard,
    ],
    data: {
      roles: ['Employer'],
    },
  },
  {
    path: 'employer/vacancies',
    component: VacancyManagementComponent,
    canActivate: [
      authGuard,
      roleGuard,
    ],
    data: {
      roles: ['Employer'],
    },
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];
