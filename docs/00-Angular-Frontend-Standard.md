4. Folder Responsibilities

Angular-ல் எந்த வகையான file எந்த folder-க்குள் இருக்க வேண்டும் என்பதைச் சொல்கிறது.

core     → முழு application-க்கும் பொதுவான logic
shared   → மீண்டும் பயன்படுத்தப்படும் UI
features → ஒவ்வொரு business module

உதாரணம்:

auth.service.ts  → core/services
header           → shared/components
login            → features/auth
5. Role-Based Routes

ஒவ்வொரு user role-க்கும் எந்த URL பயன்படுத்தப்படும் என்பதைச் சொல்கிறது.

/seeker
/employer
/admin

Job Seeker ஒருவர் /admin page-க்கு செல்லாமல் Guard தடுக்கும்.

6. Main Routes

Project-ல் உருவாக்க வேண்டிய முக்கிய pages மற்றும் URLs பட்டியல்.

உதாரணம்:

/login
/register
/jobs
/seeker/profile
/employer/vacancies
/admin/dashboard

இவை இப்போது உருவாக்க வேண்டியதில்லை. Coding steps வரும்போது உருவாக்குவோம்.

7. Forms Standard

எந்த form-க்கு எந்த Angular Forms method பயன்படுத்த வேண்டும் என்பதைச் சொல்கிறது.

Template-driven Forms

Simple forms:

Login
Job Filter
Status dropdown
Contact message
Reactive Forms

Complex forms:

Registration
Job Seeker Profile
CV Upload
Company Profile
Vacancy Form
8. Service Standard

Backend API-யுடன் பேச Angular Services பயன்படுத்த வேண்டும்.

உதாரணம்:

auth.service.ts
vacancy.service.ts
application.service.ts

Component-க்குள் எல்லா API code-ஐயும் எழுதக்கூடாது.

9. HTTP Standard

Angular Frontend எவ்வாறு Backend API-க்கு request அனுப்ப வேண்டும் என்பதைச் சொல்கிறது.

பயன்படுத்துவது:

HttpClient
HttpParams
JWT Interceptor
FormData
Typed Interfaces
environment.apiUrl
10. Matching Score Rule

இது மிக முக்கியமான rule.

Match Score → C# Backend மட்டும் calculate செய்யும்
Angular → API கொடுக்கும் score-ஐ display மட்டும் செய்யும்

Angular/TypeScript-ல் score calculate செய்யக்கூடாது.

11. Component Communication

ஒரு Angular Component மற்றொரு Component-க்கு data அனுப்புவது பற்றி.

உதாரணம்:

Job List
   ↓ job data
Job Card
   ↑ Apply event

இதற்காக:

@Input
@Output

பயன்படுத்தப்படும்.

12. Angular Template Standard

Angular HTML files-ல் data எப்படி display செய்ய வேண்டும் என்பதற்கான rules.

@if  → condition
@for → list
ngClass → condition அடிப்படையில் CSS class
ngStyle → calculated style

உதாரணம்:

@if (jobs.length === 0) {
  <p>No jobs found</p>
}
13. Custom Pipes மற்றும் Directives

Display செய்யப்படும் data-ஐ மாற்ற அல்லது UI behaviour கொடுக்க பயன்படும் Angular features.

உதாரணம்:

matchBand → 85 score-ஐ “Strong Match” என்று காட்டலாம்
shortText → மிக நீளமான text-ஐ சுருக்கலாம்
appScoreColor → score அடிப்படையில் colour கொடுக்கலாம்

BRD குறிப்பிட்டிருப்பதால் coding phase-ல் உருவாக்குவோம்.

14. UI State Standard

ஒவ்வொரு data page-க்கும் நான்கு நிலைகள் இருக்க வேண்டும்:

Loading
Error
Empty
Data available

உதாரணம்:

Loading jobs...

Jobs இல்லை:

No jobs found

API error:

Unable to load jobs

Data வந்தால் jobs list காட்டப்படும்.

15. HTTP Error Handling

Backend API தரும் error codes-ஐ Angular எப்படி handle செய்ய வேண்டும் என்பதைக் கூறுகிறது.

400 → Form data தவறு
401 → Login தேவை
403 → Permission இல்லை
404 → Record கிடைக்கவில்லை
409 → Duplicate application
500 → Server error
16. Styling Standard

Project design எப்படி இருக்க வேண்டும் என்பதைச் சொல்கிறது.

நமது design:

Light blue
White
Professional recruitment theme
Tailwind CSS
Consistent spacing
Clear validation messages
Responsive design

Dark hacking-style design பயன்படுத்த வேண்டாம்.

17. Responsive Design

Website laptop மட்டும் இல்லாமல் mobile screen-லும் சரியாகத் தெரிய வேண்டும்.

Mobile-ல்:

Sidebar collapse ஆக வேண்டும்.
Forms ஒன்றின் கீழ் ஒன்று வர வேண்டும்.
Text overlap ஆகக்கூடாது.
Buttons பயன்படுத்தக்கூடியதாக இருக்க வேண்டும்.

இது mobile application அல்ல; responsive website மட்டுமே.

18. File Naming Standard

Angular files-க்கு எப்படி பெயர் வைக்க வேண்டும் என்பதற்கான rule.

சரியானது:

job-card.component.ts
auth.service.ts
auth.guard.ts

தவறானது:

JobCard.ts
AUTH_SERVICE.ts
job_card.ts

நான் code தரும்போது exact filename மற்றும் folder path தருவேன்.

19. Security Rules

Frontend coding செய்யும்போது செய்யக்கூடாத security தவறுகள்:

Password save செய்யக்கூடாது.
JWT secret frontend-ல் வைக்கக்கூடாது.
Database connection string frontend-ல் வைக்கக்கூடாது.
CV files Angular assets-ல் வைக்கக்கூடாது.
TypeScript-ல் score calculate செய்யக்கூடாது.
Unauthorized personal information காட்டக்கூடாது.
20. Git and Team Rules

ஒவ்வொரு member-மும் எந்த branch-ல் வேலை செய்ய வேண்டும் என்பதைச் சொல்கிறது.

Roshan   → feature/roshan-auth-admin
Pravina  → feature/pravina-jobseeker-profile
Thalaivi → feature/thalaivi-employer-vacancy
Piru     → feature/piru-matching-application

Members நேரடியாக main அல்லது develop branch-க்கு push செய்யக்கூடாது.

21. Definition of Done

ஒரு feature எப்போது complete என்று சொல்லலாம் என்பதற்கான checklist.

Code எழுதியது மட்டும் complete அல்ல. கீழே உள்ள அனைத்தும் முடிந்திருக்க வேண்டும்:

Component
Service
Form validation
API connection
Role security
Loading/error/empty states
Responsive design
Build success
Browser testing
Git commit/push
22. Prohibited Scope

BRD-ல் இல்லாததால் project-ல் சேர்க்கக்கூடாத features:

Real-time Chat
Email/SMS
Online Payment
AI CV Analysis
Client-side score calculation
PWA
NgRx
External job-board integration
Native mobile application