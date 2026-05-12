export const BUILD_MASTER = 15;
export const BUILD_SUB = 30;
export const BUILD_LABEL = BUILD_SUB > 0 ? `Master ${BUILD_MASTER}.${BUILD_SUB}` : `Master ${BUILD_MASTER}`;
