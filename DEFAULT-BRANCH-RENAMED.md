# Default Branch Has Been Renamed

In May of 2023 the legacy name 'master' was removed and the default branch is now 'main'. You will need to update your local:

1. `git branch -m master main`
2. `git fetch origin`
3. `git branch -u origin/main main`
4. `git remote set-head origin -a`
5. Optional: `git remote prune origin`
