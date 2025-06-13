package legacyloom.timelineservice.repository;

import legacyloom.timelineservice.model.Timeline;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.mongodb.repository.MongoRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;
import java.util.UUID;

@Repository
public interface TimelineRepository extends MongoRepository<Timeline, String> {
    Page<Timeline> findByUserIdAndIsDeletedFalse(UUID userId, Pageable pageable);
    Optional<Timeline> findByIdAndIsDeletedFalse(String id);
}

