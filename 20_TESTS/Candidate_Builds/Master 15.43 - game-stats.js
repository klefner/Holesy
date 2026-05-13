const STORAGE_KEY = 'holesyGameStatsV1';
const MAX_RECENT_RUNS = 12;

export function createEmptyGameStats() {
  return {
    schemaVersion: 1,
    totalGames: 0,
    totalDurationSeconds: 0,
    endReasons: {},
    modes: {
      timed: createModeStats(),
      lms: createModeStats(),
      waves: createModeStats(),
    },
    difficulties: {
      normal: createDifficultyStats(),
      hard: createDifficultyStats(),
      ultra: createDifficultyStats(),
    },
    recentRuns: [],
  };
}

function createModeStats() {
  return {
    games: 0,
    wins: 0,
    losses: 0,
    abandoned: 0,
    totalDurationSeconds: 0,
    bestScore: 0,
  };
}

function createDifficultyStats() {
  return {
    games: 0,
    wins: 0,
    losses: 0,
    totalDurationSeconds: 0,
  };
}

export function loadGameStats() {
  try {
    const raw = window.localStorage && window.localStorage.getItem(STORAGE_KEY);
    if (!raw) return createEmptyGameStats();
    return normalizeGameStats(JSON.parse(raw));
  } catch (e) {
    return createEmptyGameStats();
  }
}

export function saveGameStats(stats) {
  try {
    if (window.localStorage) {
      window.localStorage.setItem(STORAGE_KEY, JSON.stringify(normalizeGameStats(stats)));
    }
  } catch (e) {}
}

export function resetGameStats() {
  const empty = createEmptyGameStats();
  saveGameStats(empty);
  return empty;
}

export function recordGameRun(stats, run) {
  const next = normalizeGameStats(stats);
  const mode = normalizeMode(run.mode);
  const difficulty = normalizeDifficulty(run.difficulty);
  const durationSeconds = Math.max(0, Math.round(run.durationSeconds || 0));
  const playerScore = Math.max(0, Math.round(run.playerScore || 0));
  const outcome = run.outcome === 'win' ? 'win' : run.outcome === 'abandoned' ? 'abandoned' : 'loss';
  const reason = run.reason || 'unknown';
  const completedRun = {
    endedAt: new Date().toISOString(),
    mode,
    difficulty,
    outcome,
    reason,
    durationSeconds,
    playerScore,
    topScore: Math.max(0, Math.round(run.topScore || 0)),
    winnerName: run.winnerName || '',
    wave: Math.max(0, Math.round(run.wave || 0)),
  };

  next.totalGames += 1;
  next.totalDurationSeconds += durationSeconds;
  next.endReasons[reason] = (next.endReasons[reason] || 0) + 1;

  const modeStats = next.modes[mode];
  modeStats.games += 1;
  modeStats.totalDurationSeconds += durationSeconds;
  modeStats.bestScore = Math.max(modeStats.bestScore || 0, playerScore);
  if (outcome === 'win') modeStats.wins += 1;
  else if (outcome === 'abandoned') modeStats.abandoned += 1;
  else modeStats.losses += 1;

  const difficultyStats = next.difficulties[difficulty];
  difficultyStats.games += 1;
  difficultyStats.totalDurationSeconds += durationSeconds;
  if (outcome === 'win') difficultyStats.wins += 1;
  else if (outcome === 'loss') difficultyStats.losses += 1;

  next.recentRuns.unshift(completedRun);
  next.recentRuns = next.recentRuns.slice(0, MAX_RECENT_RUNS);
  saveGameStats(next);
  return next;
}

export function formatReasonLabel(reason) {
  const labels = {
    score_win: 'Score win',
    score_loss: 'Score loss',
    eaten_by_rival: 'Eaten by rival',
    shot_by_soldiers: 'Shot by soldiers',
    survival_win: 'Survival win',
    survival_loss: 'Survival loss',
    waves_survived: 'Waves survived',
    waves_lockdown_complete_score_win: 'Waves score win',
    waves_lockdown_complete_score_loss: 'Waves score loss',
    abandoned: 'Abandoned',
    unknown: 'Unknown',
  };
  return labels[reason] || reason.replace(/_/g, ' ');
}

export function summarizeStats(stats) {
  const safeStats = normalizeGameStats(stats);
  const avgDuration = safeStats.totalGames
    ? Math.round(safeStats.totalDurationSeconds / safeStats.totalGames)
    : 0;
  return {
    totalGames: safeStats.totalGames,
    avgDuration,
    modes: safeStats.modes,
    difficulties: safeStats.difficulties,
    endReasons: safeStats.endReasons,
    recentRuns: safeStats.recentRuns,
  };
}

function normalizeGameStats(stats) {
  const base = createEmptyGameStats();
  if (!stats || typeof stats !== 'object') return base;
  base.totalGames = Math.max(0, Number(stats.totalGames) || 0);
  base.totalDurationSeconds = Math.max(0, Number(stats.totalDurationSeconds) || 0);
  base.endReasons = stats.endReasons && typeof stats.endReasons === 'object' ? { ...stats.endReasons } : {};
  for (const mode of Object.keys(base.modes)) {
    base.modes[mode] = { ...base.modes[mode], ...(stats.modes && stats.modes[mode] ? stats.modes[mode] : {}) };
  }
  for (const difficulty of Object.keys(base.difficulties)) {
    base.difficulties[difficulty] = {
      ...base.difficulties[difficulty],
      ...(stats.difficulties && stats.difficulties[difficulty] ? stats.difficulties[difficulty] : {}),
    };
  }
  base.recentRuns = Array.isArray(stats.recentRuns) ? stats.recentRuns.slice(0, MAX_RECENT_RUNS) : [];
  return base;
}

function normalizeMode(mode) {
  if (mode === 'lms' || mode === 'waves' || mode === 'timed') return mode;
  return 'timed';
}

function normalizeDifficulty(difficulty) {
  if (difficulty === 'hard' || difficulty === 'ultra' || difficulty === 'normal') return difficulty;
  return 'normal';
}
