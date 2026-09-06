export interface Vacancy {
  id: string;
  employerProfileId: string;
  companyName: string;
  jobTitle: string;
  department: string;
  location: string;
  workplaceType: string;
  employmentType: string;
  experienceLevel: string;
  jobDescription: string;
  requirements: string;
  requiredSkills: string;
  minimumSalary: number;
  maximumSalary: number;
  currency: string;
  applicationClosingDate: string;
  numberOfOpenings: number;
  status: string;
  postedAtUtc: string | null;
  createdAtUtc: string;
  updatedAtUtc: string;
}

export interface CreateVacancy {
  employerProfileId: string;
  jobTitle: string;
  department: string;
  location: string;
  workplaceType: string;
  employmentType: string;
  experienceLevel: string;
  jobDescription: string;
  requirements: string;
  requiredSkills: string;
  minimumSalary: number;
  maximumSalary: number;
  currency: string;
  applicationClosingDate: string;
  numberOfOpenings: number;
  status: string;
}

export interface UpdateVacancy {
  jobTitle: string;
  department: string;
  location: string;
  workplaceType: string;
  employmentType: string;
  experienceLevel: string;
  jobDescription: string;
  requirements: string;
  requiredSkills: string;
  minimumSalary: number;
  maximumSalary: number;
  currency: string;
  applicationClosingDate: string;
  numberOfOpenings: number;
  status: string;
}