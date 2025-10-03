# Branch Protection Setup for Main Branch

## 🛡️ GitHub Branch Protection Configuration

### Repository: Thaprobid-Auction-Website-API-
### Protected Branch: `main`

## 🔒 Protection Rules

### 1. **Require Pull Request Reviews**
- ✅ **Require pull request reviews before merging**
- ✅ **Required number of reviewers**: 1 (minimum)
- ✅ **Dismiss stale pull request approvals when new commits are pushed**
- ✅ **Require review from code owners** (when CODEOWNERS file exists)
- ✅ **Restrict pushes that create files that exceed the path length**

### 2. **Require Status Checks**
- ✅ **Require status checks to pass before merging**
- ✅ **Require branches to be up to date before merging**

**Required Status Checks:**
- `CI/CD Pipeline / test` (Build and test job)
- `CI/CD Pipeline / security` (Security checks)
- `Security Scan / dependency-scan`
- `Security Scan / code-analysis`
- `Security Scan / secret-scan`

### 3. **Restrict Pushes**
- ✅ **Restrict pushes that create files**
- ✅ **Only allow specific people and teams to push**

**Allowed to push:**
- Repository administrators
- @kavindukaveesha (repository owner)

### 4. **Force Push Protection**
- ✅ **Do not allow force pushes**
- ✅ **Do not allow deletions**

### 5. **Additional Restrictions**
- ✅ **Include administrators** (applies rules to admins too)
- ✅ **Require linear history** (no merge commits)
- ✅ **Allow force pushes to matching branches**: ❌ Disabled

## 🚀 GitHub CLI Commands to Set Up Protection

```bash
# Navigate to repository
cd "/Users/kavindu/Developer/Software Enginner/Projects/Next-Era-Solutions/Thaprobid-Auction-Website-API-"

# Enable branch protection for main branch
gh api repos/:owner/:repo/branches/main/protection \
  --method PUT \
  --field required_status_checks='{"strict":true,"contexts":["CI/CD Pipeline / test","CI/CD Pipeline / security","Security Scan / dependency-scan","Security Scan / code-analysis"]}' \
  --field enforce_admins=true \
  --field required_pull_request_reviews='{"required_approving_review_count":1,"dismiss_stale_reviews":true,"require_code_owner_reviews":true}' \
  --field restrictions='{"users":["kavindukaveesha"],"teams":[]}' \
  --field allow_force_pushes=false \
  --field allow_deletions=false
```

## 🔧 Manual Setup via GitHub Web Interface

### Step 1: Navigate to Settings
1. Go to repository: `https://github.com/kavindukaveesha/Thaprobid-Auction-Website-API-`
2. Click **Settings** tab
3. Click **Branches** in the left sidebar

### Step 2: Add Branch Protection Rule
1. Click **Add rule**
2. Branch name pattern: `main`

### Step 3: Configure Protection Settings

#### **Protect matching branches:**
- ☑️ Require a pull request before merging
  - ☑️ Require approvals: **1**
  - ☑️ Dismiss stale pull request approvals when new commits are pushed
  - ☑️ Require review from code owners

#### **Require status checks before merging:**
- ☑️ Require status checks to pass before merging
- ☑️ Require branches to be up to date before merging
- Add status checks:
  - `CI/CD Pipeline / test`
  - `CI/CD Pipeline / security`
  - `Security Scan / dependency-scan`
  - `Security Scan / code-analysis`

#### **Restrict pushes that create files:**
- ☑️ Restrict pushes that create files that exceed GitHub's file size limit

#### **Rules applied to everyone including administrators:**
- ☑️ Include administrators
- ☑️ Allow force pushes: **❌ NEVER**
- ☑️ Allow deletions: **❌ NEVER**

## 📋 CODEOWNERS File

Create `.github/CODEOWNERS` file:

```
# Global code owners
* @kavindukaveesha

# API specific files
/api/ @kavindukaveesha

# Security sensitive files
/api/appsettings*.json @kavindukaveesha
/api/Program.cs @kavindukaveesha
/.github/workflows/ @kavindukaveesha
/Dockerfile @kavindukaveesha
/docker-compose.yml @kavindukaveesha

# Documentation
*.md @kavindukaveesha
/docs/ @kavindukaveesha
```

## 🔍 Required Status Checks Setup

### 1. **Enable GitHub Actions**
Ensure the following workflows are enabled:
- `.github/workflows/ci-cd.yml`
- `.github/workflows/security-scan.yml`

### 2. **Security Features**
Enable in repository settings:
- ☑️ **Dependency graph**
- ☑️ **Dependabot alerts**
- ☑️ **Dependabot security updates**
- ☑️ **Code scanning alerts**
- ☑️ **Secret scanning alerts**

### 3. **Branch Protection Status Checks**
Required checks that must pass:
1. **Build and Test** - Ensures code compiles and tests pass
2. **Security Scan** - Checks for vulnerabilities
3. **Code Analysis** - Static code analysis
4. **Dependency Check** - Scans for vulnerable dependencies

## 🚨 Security Enforcement Rules

### **Direct Commits to Main:**
- ❌ **BLOCKED** - All changes must go through PRs

### **Unreviewed Changes:**
- ❌ **BLOCKED** - Minimum 1 review required

### **Failed Security Checks:**
- ❌ **BLOCKED** - All security checks must pass

### **Outdated Branches:**
- ❌ **BLOCKED** - Branches must be up-to-date with main

### **Force Push/Deletion:**
- ❌ **BLOCKED** - Never allowed, even for admins

## 🔧 Additional Security Settings

### **Repository Security Settings:**
```bash
# Enable vulnerability reporting
gh repo edit --enable-vulnerability-reporting

# Enable private vulnerability reporting
gh repo edit --enable-private-vulnerability-reporting

# Enable security and analysis features
gh api repos/:owner/:repo \
  --method PATCH \
  --field security_and_analysis='{"dependency_graph":{"status":"enabled"},"dependabot_security_updates":{"status":"enabled"},"secret_scanning":{"status":"enabled"},"secret_scanning_push_protection":{"status":"enabled"}}'
```

### **Webhook Configuration:**
Set up webhooks for:
- Security alerts
- Pull request events
- Push events to protected branches

## 📊 Monitoring and Compliance

### **Regular Reviews:**
- Weekly: Review failed PR attempts
- Monthly: Audit branch protection compliance
- Quarterly: Update protection rules based on threats

### **Metrics to Track:**
- Number of direct push attempts (should be 0)
- PR review compliance rate (should be 100%)
- Security check failure rate
- Time from PR creation to merge

### **Alerts Setup:**
- Failed security checks
- Attempted force pushes
- Branch protection rule violations

## ✅ Verification Checklist

After setting up branch protection:

- [ ] Attempt direct push to main (should fail)
- [ ] Create PR without review (should require approval)
- [ ] Try to merge PR with failing tests (should block)
- [ ] Verify security checks are running
- [ ] Test force push prevention
- [ ] Confirm CODEOWNERS review requirements

## 🔄 Rollback Plan

If issues arise with branch protection:

1. **Temporary Disable:**
   ```bash
   gh api repos/:owner/:repo/branches/main/protection --method DELETE
   ```

2. **Selective Rule Removal:**
   - Remove specific status checks
   - Temporarily allow admin override
   - Adjust review requirements

3. **Emergency Access:**
   - Admin can override in critical situations
   - Must document reason and restore protection

## 📞 Support and Troubleshooting

### **Common Issues:**
1. **Status checks not appearing** - Ensure workflows run at least once
2. **CODEOWNERS not working** - Check file syntax and permissions
3. **Admin bypass not working** - Verify "Include administrators" setting

### **Documentation:**
- [GitHub Branch Protection](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/defining-the-mergeability-of-pull-requests/about-protected-branches)
- [Status Checks](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/collaborating-on-repositories-with-code-quality-features/about-status-checks)
- [CODEOWNERS](https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/customizing-your-repository/about-code-owners)