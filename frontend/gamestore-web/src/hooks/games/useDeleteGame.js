import { useMutation,
  useQueryClient }
from "@tanstack/react-query";

import { deleteGame }
from "../../api/gamesApi";

export function useDeleteGame() {
  const queryClient =
    useQueryClient();

  return useMutation({
    mutationFn: deleteGame,

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["games"],
      });
    },
  });
}