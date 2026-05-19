import { useQuery }
  from "@tanstack/react-query";

import { getGameById }
  from "../../api/gamesApi";

export function useGameById(id) {
  return useQuery({
    queryKey: ["game", id],

    queryFn: () => getGameById(id),

    enabled: Boolean(id),

    staleTime: 1000 * 60,

    retry: 1,
  });
}
